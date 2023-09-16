namespace SelfDevelopmentProj.Workers.Infrastructure
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task<bool> WaitWithCancelation(this SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
			try
			{
				await semaphore.WaitAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				return false;
			}

			return true;
        }
    }
}
