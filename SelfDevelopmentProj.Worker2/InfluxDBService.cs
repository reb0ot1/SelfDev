using InfluxDB.Client;
using Microsoft.Extensions.Logging;

namespace SelfDevelopmentProj.Worker2
{
    public class InfluxDBService
    {
        private readonly string _token = "W6QLf21pdQ6QM86YsPG7kAoiSRE3IwzoRIpOok25di9pqubvqVpnRU5lZBezFSGc8whYvDo9fMCb3dfPmYm-Cg==";
        private readonly ILogger _logger;

        public InfluxDBService(ILogger<InfluxDBService> logger)
        {
            this._logger = logger;
        }

        public void Write(Action<WriteApi> action)
        {
            try
            {
                using var client = new InfluxDBClient("http://localhost:8086", _token);
                using var write = client.GetWriteApi();

                action(write);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception inf service.");
            }
        }

        public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
        { 
            using var client = new InfluxDBClient("http://localhost:8086", _token);
            var query = client.GetQueryApi();
            return await action(query);
        }
    }
}
