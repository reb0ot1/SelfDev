using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SelfDevelopmentProj.Workers.Data;
using SelfDevelopmentProj.Workers.Infrastructure;
using System.Threading.Channels;

namespace SelfDevelopmentProj.Workers.DataProcessing
{
    public partial class BackgroundDataProcessor : BackgroundService, IDataProcessor
    {
        private readonly Channel<DataWithKey> _internalQueue = Channel.CreateUnbounded<DataWithKey>(new UnboundedChannelOptions { SingleReader = true });
        private readonly Dictionary<int, KeySpecificDataProcessor> _dataProcessing = new();
        private readonly SemaphoreSlim _processorsLock = new (1, 1);
        private BackgroundDataProcessorMonitor? _monitor;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<BackgroundDataProcessor> _logger;

        public BackgroundDataProcessor(IServiceScopeFactory serviceFactory, ILoggerFactory loggerFactory)
        {
            this._serviceScopeFactory = serviceFactory;
            this._loggerFactory = loggerFactory;
            this._logger = _loggerFactory.CreateLogger<BackgroundDataProcessor>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._monitor = BackgroundDataProcessorMonitor.CreateAndStart(
                this._processorsLock,
                this._dataProcessing,
                this._loggerFactory.CreateLogger<BackgroundDataProcessorMonitor>(),
                stoppingToken);

            await foreach (var data in this._internalQueue.Reader.ReadAllAsync(stoppingToken))
            {
                if (!await this._processorsLock.WaitWithCancelation(stoppingToken))
                {
                    break;
                }

                var processor = this.GetOrCreateDataProcessor(data, stoppingToken);
                await processor.ScheduleDataProcessing(data);

                this._processorsLock.Release();
                this._logger.LogInformation("Scheduled new data '{Data}' for processor with key '{Key}'", data, data.Key);
            }

            await this._monitor.StopMonitoring();
        }

        private KeySpecificDataProcessor GetOrCreateDataProcessor(DataWithKey data, CancellationToken processorCancellationToken = default)
        {
            var logger = this._loggerFactory.CreateLogger<KeySpecificDataProcessor>();
            var instance = KeySpecificDataProcessor.CreateAndStartProcessing(
                data.Key,
                this._serviceScopeFactory,
                logger, 
                processorCancellationToken);

            return instance;
        }

        public async Task ScheduledDataProcessing(DataWithKey dataWithKey)
            => await this._internalQueue.Writer.WriteAsync(dataWithKey);
    }
}
