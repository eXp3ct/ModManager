using NLog;

namespace Logging
{
    public static class LoggerService
    {
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static LoggerService()
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"NLog.config");
        }
    }
}