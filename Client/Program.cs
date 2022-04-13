global using WasmTwoWayLogging.Client.Logging;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WasmTwoWayLogging.Client;
using WasmTwoWayLogging.Client.Library;
using WasmTwoWayLogging.Client.Services;

Logger.Configure();
Logger.Log.Info("App starting.");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<DebugService>();
builder.Services.AddScoped<JsConsole>();

await builder.Build().RunAsync();
