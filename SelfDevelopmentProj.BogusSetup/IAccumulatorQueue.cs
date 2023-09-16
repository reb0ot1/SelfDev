using System.Diagnostics.CodeAnalysis;

namespace SelfDevelopmentProj.BogusSetup
{
    public interface IAccumulatorQueue
    {
        ValueTask PushAsync([NotNull] string data);

        ValueTask<string> PullAsync(CancellationToken cancellationToken);
    }
}
