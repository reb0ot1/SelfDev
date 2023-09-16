using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SelfDevelopmentProj.Workers.Data;
using SelfDevelopmentProj.Workers.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SelfDevelopmentProj.Workers.DataProcessing
{
    public class KeySpecificDataProcessor : IDataProcessor
    {
        public int ProcessorKey { get;}

        public DateTime LastProcesingTimestamp => _processingFinishedTimestamp ?? DateTime.UtcNow;

        private DateTime? _processingFinishedTimestamp = DateTime.UtcNow;

        private bool Processing
        {
            set {
                if (!value)
                {
                    _processingFinishedTimestamp = DateTime.UtcNow;
                }
                else
                {
                    _processingFinishedTimestamp = null;
                }
            }
        }

        private Task? _processingTask;

        private readonly Channel<DataWithKey> _internalQueue = Channel.CreateUnbounded<DataWithKey>(new UnboundedChannelOptions { SingleReader = true, SingleWriter = true });

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger _logger;

        private KeySpecificDataProcessor(int processorKey, IServiceScopeFactory serviceScopeFactory, ILogger logger)
        {
            ProcessorKey = processorKey;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        private void StartProcessing(CancellationToken cancellationToken = default)
        {
            _processingTask = Task.Factory.StartNew(
                async () =>
                {
                    await foreach (var data in this._internalQueue.Reader.ReadAllAsync(cancellationToken))
                    {
                        Processing = true;
                        this._logger.LogInformation("Received data: {Data}", data);
                        using (var dependenciesProvider = new DependenciesProvider(_serviceScopeFactory))
                        {
                            await ProcessData(data, dependenciesProvider.Dependency);
                        }

                        this.Processing = this._internalQueue.Reader.TryPeek(out _);
                    }
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task ProcessData(DataWithKey data, IDependency dependency)
        {
            await dependency.DoStuff();
        }

        public async Task ScheduleDataProcessing(DataWithKey dataWithKey)
        {
            if (dataWithKey.Key != this.ProcessorKey)
            {
                throw new InvalidOperationException($"Data with key {dataWithKey.Key} scheduled for KeySpecificDataProcessor with key {this.ProcessorKey}.");
            }

            this.Processing = true;
            await this._internalQueue.Writer.WriteAsync(dataWithKey);
        }

        public static KeySpecificDataProcessor CreateAndStartProcessing(
            int processorKey,
            IServiceScopeFactory scopeFactory,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var instance = new KeySpecificDataProcessor(processorKey, scopeFactory, logger);
            instance.StartProcessing();
            return instance;
        }

        public async Task ScheduledDataProcessing(DataWithKey dataWithKey)
        {
            if (dataWithKey.Key != ProcessorKey)
            {
                throw new InvalidOperationException($"Data with key {dataWithKey.Key} scheduled for KeySpecificDataProcessor with key {ProcessorKey}");
            }

            Processing = true;
            await _internalQueue.Writer.WriteAsync(dataWithKey);
        }

        public async Task StopProcessing()
        {
            this._internalQueue.Writer.Complete();
            if (this._processingTask != null)
            {
                await this._processingTask;
            }
        }

        private class DependenciesProvider : IDisposable
        {
            private readonly IServiceScope _serviceScope;
            public IDependency Dependency { get; }

            public DependenciesProvider(IServiceScopeFactory serviceScopeFactory)
            {
                this._serviceScope = serviceScopeFactory.CreateScope();
                Dependency = _serviceScope.ServiceProvider.GetRequiredService<IDependency>();
            }

            public void Dispose()
            {
                _serviceScope.Dispose();
            }
        }
    }
}
