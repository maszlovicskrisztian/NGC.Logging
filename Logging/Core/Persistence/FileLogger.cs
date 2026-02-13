namespace NGC.Logging.Core
{
    using Microsoft.Extensions.Configuration;
    using NGC.Logging.Objects;

    internal class FileLogger : ILogger
    {
        public FileLogger(IConfiguration configuration)
        {
        }

        public void LogError(string module, int? sessionId, string message)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Error.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Error);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogInfo(string module, int? sessionId, string message)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Info.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Info);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogWarning(string module, int? sessionId, string message)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Warning.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Warning);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Log(string module, int? sessionId, string message, string logLevelKeyword)
        {
            try
            {
                if (!CheckLogLevelSetting(logLevelKeyword))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message);
                log.LogLevelKeyword = logLevelKeyword;

                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveLog(LogDto log)
        {
            if (string.IsNullOrEmpty(LogSettings.LogFilePath))
                return;

            LogHandler.ValidateLog(log);
            // Get stack trace info
            var stackTrace = new System.Diagnostics.StackTrace();
            var callerMethod = stackTrace.GetFrame(2)?.GetMethod();
            var callerClass = callerMethod?.DeclaringType?.FullName;

            var logEntry = $"{log.CreatedAt:yyyy-MM-dd HH:mm:ss}";
            logEntry += log.LogLevel.HasValue ? $"|{log.LogLevel.Value.ToString()}" : $"|{log.LogLevelKeyword}";
            logEntry += string.IsNullOrEmpty(log.Module) ? "|Module: N/A" : $"|Module: {log.Module}";
            logEntry += $"|SessionId: {log.SessionId}";
            logEntry += $"|{callerClass}.{callerMethod?.Name}";
            logEntry += $" - {log.Message}{Environment.NewLine}";


            Directory.CreateDirectory(LogSettings.LogFilePath);
            var logFile = Path.Combine(LogSettings.LogFilePath, $"{DateTime.Now:yyyyMMdd}_TubeSystem.log");
            File.AppendAllText(logFile, logEntry);
        }

        private bool CheckLogLevelSetting(string logLevel)
        {
            // If the setting is not configured, file logging is turned off.
            var fileMinLogLevel = LogSettings.FileLogMinLevel;
            if (string.IsNullOrEmpty(fileMinLogLevel))
                return false;

            if (Enum.TryParse<LogLevels>(fileMinLogLevel, true, out var fileMinLogLevelValue))
            {
                if (!Enum.TryParse<LogLevels>(logLevel, true, out var logLevelEnum))
                    return true;

                return logLevelEnum >= fileMinLogLevelValue;
            }

            if (fileMinLogLevel.Equals(logLevel, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
