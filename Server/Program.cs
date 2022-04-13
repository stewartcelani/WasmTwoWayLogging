global using WasmTwoWayLogging.Server.Logging;
using Microsoft.AspNetCore.ResponseCompression;
using WasmTwoWayLogging.Server.Hubs;
using WasmTwoWayLogging.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddSingleton<DebugService>();

var app = builder.Build();

// Need to get reference to Server\Services\DebugService.cs to inject into Logging\Logger.cs::Configure
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    DebugService debugService = services.GetRequiredService<DebugService>();
    Logger.Configure(debugService);
    Logger.Log.Info("App starting");
    // Lets log something every second to simulate legitimate log output
    System.Timers.Timer timer = new System.Timers.Timer(3000);
    timer.Elapsed += (source, e) =>
    {
        Logger.Log.Trace("tick");
    };
    timer.Enabled = true;
    timer.Start();
}


app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<LoggingHub>("/hubs/logging");
app.MapFallbackToFile("index.html");

app.Run();