using Microsoft.AspNetCore.SignalR.Client;

namespace WasmTwoWayLogging.Shared
{
    public class InfiniteRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // For the first 5 minutes try to reconnect every 15 seconds and after that try every 60
            if (retryContext.ElapsedTime < TimeSpan.FromSeconds(300))
            {
                return TimeSpan.FromSeconds(15);
            }
            else
            {
                return TimeSpan.FromSeconds(60);
            }
        }
    }
}
