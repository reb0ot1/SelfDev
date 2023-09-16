using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace SelfDevelopmentProj.BogusSetup
{
    public class AccumulatorQueue : IAccumulatorQueue
    {
        private readonly Channel<string> _queue;
        private readonly ILogger<AccumulatorQueue> _logger;

        public AccumulatorQueue(ILogger<AccumulatorQueue> logger)
        {
            this._logger = logger;
            var opts = new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait };
            _queue = Channel.CreateBounded<string>(opts);
        }

        public async ValueTask<string> PullAsync(CancellationToken cancellationToken)
        {
            var readData = await this._queue.Reader.ReadAsync(cancellationToken);
            Console.WriteLine("Do some work");
            Thread.Sleep(3000);

            this._logger.LogInformation(string.Format("Data [{0}] is removed from queue.", readData));

            return readData;
        }

        public async ValueTask PushAsync([NotNull] string data)
        {
            await this._queue.Writer.WriteAsync(data);

            this._logger.LogInformation(string.Format("Data [{0}] is in queue.", data));
            
        }
    }
}
