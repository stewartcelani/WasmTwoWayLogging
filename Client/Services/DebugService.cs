using WasmTwoWayLogging.Client.Library;

namespace WasmTwoWayLogging.Client.Services
{
    public class DebugService
    {
        private readonly JsConsole _console;

        public DebugService(JsConsole console)
        {
            Logger.Log.Trace("Ctor");
            _console = console;
        }

        public async Task HandleLogFromServer(string logMessage)
        {
            await _console.LogAsync(logMessage);
        }
    }
}
