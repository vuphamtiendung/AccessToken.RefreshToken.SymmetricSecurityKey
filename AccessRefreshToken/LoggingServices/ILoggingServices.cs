namespace AccessRefreshToken.LoggingServices
{
    public interface ILoggingServices
    {
        void LogInfor(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogWarn(string message);
    }
}
