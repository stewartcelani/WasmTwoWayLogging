using NLog;
using NLog.Targets;
using WasmTwoWayLogging.Server.Services;

namespace WasmTwoWayLogging.Server.Logging
{
    public class LoggingHubTarget : AsyncTaskTarget
    {
        private readonly DebugService _debugService;
        public LoggingHubTarget(DebugService debugService)
        {
            _debugService = debugService;
            OptimizeBufferReuse = true;
        }
        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken token)
        {
            string logMessage = this.Layout.Render(logEvent);
            await _debugService.LogToBrowser(logMessage);
        }
    }
}
