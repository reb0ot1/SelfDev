using Microsoft.Extensions.Logging;

namespace SelfDevelopmentProj.Workers.Other
{
    public class Dependency : IDependency
    {
        private readonly string _dependencyId = Path.GetRandomFileName();

        private readonly ILogger<Dependency> _logger;

        public Dependency(ILogger<Dependency> logger)
        {
            _logger = logger;
        }

        public Task DoStuff()
        {
            _logger.LogInformation("Dependency {DependencyId} doing stuff", this._dependencyId);
            return Task.CompletedTask;
        }
    }
}
