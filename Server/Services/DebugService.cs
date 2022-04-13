using Microsoft.AspNetCore.SignalR;
using WasmTwoWayLogging.Server.Hubs;

namespace WasmTwoWayLogging.Server.Services
{
    public class DebugService
    {
        private readonly IHubContext<LoggingHub> _hubContext;
        public DebugService(IHubContext<LoggingHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task LogToBrowser(string logMessage)
        {
            await _hubContext.Clients.All.SendAsync("Log", logMessage);
        }
    }
}
