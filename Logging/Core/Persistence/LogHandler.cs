namespace NGC.Logging.Core
{
    using NGC.Logging.Objects;

    internal static class LogHandler
    {
        internal static LogDto CreateLogDto(string module, int? sessionId, string message, LogLevels? logLevel = null)
        {
            var log = new LogDto
            {
                Module = module,
                SessionId = sessionId ?? 0,
                Message = message,
                LogLevel = logLevel.HasValue ? logLevel.Value : null,
                CreatedAt = DateTime.Now
            };

            return log;
        }

        internal static void ValidateLog(LogDto log)
        {
            if (log == null)
                throw new ArgumentNullException(nameof(log), "Log cannot be null.");

            if (string.IsNullOrEmpty(log.Message))
                throw new FormatException("Log message cannot be null or empty.");

            if (!log.LogLevel.HasValue && string.IsNullOrEmpty(log.LogLevelKeyword))
                throw new FormatException("Log must have either a LogLevel or a LogLevelKeyword.");
        }
    }
}
