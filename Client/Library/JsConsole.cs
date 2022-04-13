using Microsoft.JSInterop;

namespace WasmTwoWayLogging.Client.Library
{
    public class JsConsole
    {
        private readonly IJSRuntime _jsRuntime;
        public JsConsole(IJSRuntime jSRuntime)
        {
            Logger.Log.Trace("Ctor");
            _jsRuntime = jSRuntime;
        }

        public async Task LogAsync(string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.log", message);
        }

        public async Task ClearAsync()
        {
            Logger.Log.Trace("Top");
            await _jsRuntime.InvokeVoidAsync("console.clear");
        }
    }
}
