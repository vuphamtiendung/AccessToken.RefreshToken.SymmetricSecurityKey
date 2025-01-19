using NLog;
using NLog.Web;

namespace AccessRefreshToken.LoggingServices
{
    public static class LoggingExtension
    {
        public static void LoggingConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<ILoggingServices, LoggingServices>();
        }
    }
}
