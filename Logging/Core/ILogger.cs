namespace NGC.Logging
{
    public interface ILogger
    {
        public void LogInfo(string module, int? sessionId,string message, string logBy);
        public void LogWarning(string module, int? sessionId, string message, string logBy);
        public void LogError(string module, int? sessionId, string message, string logBy);
        public void Log(string module, int? sessionId, string message, string logLevelKeyword, string logBy);
    }
}
