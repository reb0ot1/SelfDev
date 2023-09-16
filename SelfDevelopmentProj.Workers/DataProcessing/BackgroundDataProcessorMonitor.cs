using Microsoft.Extensions.Logging;
using SelfDevelopmentProj.Workers.Infrastructure;

namespace SelfDevelopmentProj.Workers.DataProcessing
{
    public partial class BackgroundDataProcessor
    {
        public class BackgroundDataProcessorMonitor
        {
            private readonly TimeSpan _processorExpiryThreshold = TimeSpan.FromSeconds(30);
            private readonly TimeSpan _processorExpiryScanningPeriod = TimeSpan.FromSeconds(5);
            private MonitoringTask? _monitoringTask;
            private readonly SemaphoreSlim _processorsLock;
            private readonly Dictionary<int, KeySpecificDataProcessor> _dataProcessors;
            private readonly ILogger<BackgroundDataProcessorMonitor> _logger;

            private BackgroundDataProcessorMonitor(
                SemaphoreSlim processorsLock,
                Dictionary<int, KeySpecificDataProcessor> dataProcessors,
                ILogger<BackgroundDataProcessorMonitor> logger
                )
            {
                this._processorsLock = processorsLock;
                this._dataProcessors = dataProcessors;
                this._logger = logger;
            }

            private void StartMonitoring(CancellationToken cancellationToken = default)
            {
                var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var task = Task.Factory.StartNew(async () =>
                {
                    using var timer = new PeriodicTimer(_processorExpiryScanningPeriod);
                    while (!tokenSource.IsCancellationRequested && await timer.WaitForNextTickAsync(tokenSource.Token))
                    {
                        if (!await _processorsLock.WaitWithCancelation(tokenSource.Token))
                        {
                            continue;
                        }

                        var expiredProcessors = _dataProcessors.Values.Where(IsExpired).ToArray();
                        foreach (var expiredProcessor in expiredProcessors)
                        {
                            await expiredProcessor.StopProcessing();
                            _dataProcessors.Remove(expiredProcessor.ProcessorKey);

                            _logger.LogInformation("Removed data processor for data key {Key}", expiredProcessor.ProcessorKey);
                        }

                        _processorsLock.Release();
                    }
                }, tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                _monitoringTask = new MonitoringTask(task, tokenSource);
            }

            private bool IsExpired(KeySpecificDataProcessor processorInfo) => (DateTime.UtcNow - processorInfo.LastProcesingTimestamp) > _processorExpiryThreshold;

            public async Task StopMonitoring()
            {
                if (this._monitoringTask.HasValue)
                {
                    if (!this._monitoringTask.Value.CancellationTokenSource.IsCancellationRequested)
                    {
                        this._monitoringTask.Value.CancellationTokenSource.Cancel();
                    }

                    await _monitoringTask.Value.Task;
                    this._monitoringTask.Value.CancellationTokenSource.Dispose();
                    this._monitoringTask = null;
                }
            }

            public static BackgroundDataProcessorMonitor CreateAndStart(
                SemaphoreSlim processorLock,
                Dictionary<int, KeySpecificDataProcessor> dataProcessors,
                ILogger<BackgroundDataProcessorMonitor> logger,
                CancellationToken monitorCancellationToken = default)
            {
                var monitor = new BackgroundDataProcessorMonitor(processorLock, dataProcessors, logger);
                monitor.StartMonitoring(monitorCancellationToken);

                return monitor;
            }

            private readonly record struct MonitoringTask(Task Task, CancellationTokenSource CancellationTokenSource);
        }
    }
}
