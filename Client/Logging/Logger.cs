using NLog;
using NLog.Config;
using NLog.Targets;

namespace WasmTwoWayLogging.Client.Logging
{
    public class Logger
    {
        public static NLog.Logger Log = LogManager.GetCurrentClassLogger();

        public static void Configure(string baseUri)
        {
            Console.WriteLine(baseUri);
            LoggingConfiguration config = new LoggingConfiguration();
            string layout = "${longdate}|Client|${level}|${callsite}|Line:${callsite-linenumber}|${message}             ${all-event-properties} ${exception:format=tostring}";

            // Log to JavascriptConsole
            ConsoleTarget consoleTarget = new ConsoleTarget()
            {
                Layout = layout
            };
            config.AddRule(minLevel: NLog.LogLevel.Trace, maxLevel: NLog.LogLevel.Fatal, target: consoleTarget);

            // Log to SignalR LoggingHub
            var loggingHubTarget = new LoggingHubTarget($"{baseUri}hubs/logging")
            {
                Layout = layout
            };
            config.AddRule(minLevel: NLog.LogLevel.Trace, maxLevel: NLog.LogLevel.Fatal, target: loggingHubTarget);

            LogManager.Configuration = config;
            Log = LogManager.GetCurrentClassLogger();
        }
    }
}
