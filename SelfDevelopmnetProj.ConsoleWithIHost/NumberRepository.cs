using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SelfDevelopmnetProj.ConsoleWithIHost
{
    public interface INumberRepository
    {
        int GetNumber(bool useDefault = false);
    }

    public class NumberRepository : INumberRepository
    {
        private readonly AppConfiguration _appConfiguration;
        private readonly ILogger<NumberRepository> _logger;

        public NumberRepository(IOptions<AppConfiguration> appConfig, ILogger<NumberRepository> logger)
        {
            this._appConfiguration = appConfig.Value;
            this._logger = logger;
        }


        public int GetNumber(bool useDefault = false)
        {
            this._logger.LogInformation("Test information.");
            this._logger.LogWarning("Test warning.");
            this._logger.LogError("Test error.");

            if (useDefault)
            {
                return this._appConfiguration.DefaultNumber;
            }

            return -42;
        }
    }
}
