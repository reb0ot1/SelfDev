namespace SelfDevelopmentProj.BogusSetup.BackgroundServices
{
    public class AccumulatorBackgroundService : BackgroundService
    {
        private readonly IAccumulatorQueue _queueService;
        private readonly ILogger<AccumulatorBackgroundService> _logger;

        public AccumulatorBackgroundService(ILogger<AccumulatorBackgroundService> logger, IAccumulatorQueue queueService)
        {
            this._logger = logger;
            this._queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var getMessage = await this._queueService.PullAsync(stoppingToken);

                    this._logger.LogInformation("Message received successfully.");
                }
                catch (Exception ex)
                {
                    this._logger.LogError("Failed to receive message.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Stop backgroundService");
            await base.StopAsync(cancellationToken);
        }
    }
}
