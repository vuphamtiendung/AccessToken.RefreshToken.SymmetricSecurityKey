using NLog;
using NLog.Web;

using ILogger = NLog.ILogger;

namespace AccessRefreshToken.LoggingServices
{
    public class LoggingServices : ILoggingServices
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message) => _logger.Debug(message);
        public void LogError(string message) => _logger.Error(message);
        public void LogInfor(string message) => _logger.Info(message);
        public void LogWarn(string message) => _logger.Warn(message);
    }
}
