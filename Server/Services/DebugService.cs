using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using NLog.Targets;
using WasmTwoWayLogging.Server.Hubs;
using WasmTwoWayLogging.Shared;

namespace WasmTwoWayLogging.Server.Services
{
    public class DebugService
    {
        private readonly string _loggingHubUrl = "https://localhost:7154/hubs/logging";
        private readonly IHubContext<LoggingHub> _hubContext;
        private HubConnection? _hubConnection;

        public DebugService(IHubContext<LoggingHub> hubContext)
        {
            Logger.Log.Trace("Ctor");
            _hubContext = hubContext;
            EnsureConnection();
        }

        public async void EnsureConnection()
        {
            Logger.Log.Trace("Top");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_loggingHubUrl)
                .WithAutomaticReconnect(new InfiniteRetryPolicy())
                .Build();

            _hubConnection.On<string>("LogToServer", HandleLogFromBrowser);
            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                try
                {
                    await _hubConnection.StopAsync();
                    await _hubConnection.DisposeAsync();
                }
                catch (Exception ex)
                {
                    NLog.Common.InternalLogger.Error(ex, "Exception in LoggingHubConnection.DisposeAsync");
                }
                finally
                {
                    _hubConnection = null;
                }
            }
        }

        public void HandleLogFromBrowser(string logMessage)
        {
            // Want to blend in to NLog ColoredConsole
            if (logMessage.Contains("|Trace|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.DarkGray;
            }
            else if (logMessage.Contains("|Debug|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.Gray;
            }
            else if (logMessage.Contains("|Info|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.White;
            }
            else if (logMessage.Contains("|Warn|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.Yellow;
            }
            else if (logMessage.Contains("|Error|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.Red;
            }
            else if (logMessage.Contains("|Fatal|"))
            {
                Console.ForegroundColor = (ConsoleColor)ConsoleOutputColor.Red;
                Console.BackgroundColor = (ConsoleColor)ConsoleOutputColor.White;
            }
            Console.WriteLine(logMessage);
            Console.ResetColor();
        }

        public async Task LogToBrowser(string logMessage)
        {
            await _hubContext.Clients.All.SendAsync("LogToBrowser", logMessage);
        }
    }
}
