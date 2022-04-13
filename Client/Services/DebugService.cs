using WasmTwoWayLogging.Client.Library;

namespace WasmTwoWayLogging.Client.Services
{
    public class DebugService
    {
        private readonly JsConsole _console;
        public event Func<Task>? OnChange;
        public bool ConnectToServerConsole = false;
        public List<string> LogMessages = new List<string>();

        public DebugService(JsConsole console)
        {
            Logger.Log.Trace("Ctor");
            _console = console;
        }

        public async Task AddLogMessage(string logMessage)
        {
            LogMessages.Add(logMessage);
            await _console.LogAsync(logMessage);
            HandleOnChange();
        }

        public async Task ClearLogMessages()
        {
            Logger.Log.Trace("Top");
            LogMessages.Clear();
            await _console.ClearAsync();
            HandleOnChange();
        }

        public void ToggleConnectToServerConsole()
        {
            Logger.Log.Trace("Top");
            ConnectToServerConsole = !ConnectToServerConsole;
            HandleOnChange();
        }

        private void HandleOnChange()
        {
            if (OnChange != null)
            {
                OnChange?.Invoke();
            }
        }
    }
}
