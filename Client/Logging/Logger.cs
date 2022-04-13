using NLog;
using NLog.Config;
using NLog.Targets;

namespace WasmTwoWayLogging.Client.Logging
{
    public class Logger
    {
        public static NLog.Logger Log = LogManager.GetCurrentClassLogger();

        public static void Configure()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            string layout = "${longdate}|Client|${level}|${callsite}|Line:${callsite-linenumber}|${message}             ${all-event-properties} ${exception:format=tostring}";

            ConsoleTarget consoleTarget = new ConsoleTarget()
            {
                Layout = layout
            };
            config.AddRule(minLevel: NLog.LogLevel.Trace, maxLevel: NLog.LogLevel.Fatal, target: consoleTarget);

            LogManager.Configuration = config;
            Log = LogManager.GetCurrentClassLogger();
        }
    }
}
