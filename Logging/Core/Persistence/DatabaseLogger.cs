namespace NGC.Logging.Core
{
    using NGC.Logging.DataAccess.Repositories;
    using NGC.Logging.Mapping;
    using NGC.Logging.Objects;

    internal class DatabaseLogger : ILogger
    {
        private readonly ILogRepository logRepository;
        private readonly ILogLevelRepository logLevelRepository;

        internal DatabaseLogger(ILogRepository logRepository, ILogLevelRepository logLevelRepository)
        {
            this.logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
            this.logLevelRepository = logLevelRepository ?? throw new ArgumentNullException(nameof(logLevelRepository));
        }

        public void LogError(string module, int? sessionId, string message, string logBy)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Error.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Error, logBy);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogInfo(string module, int? sessionId, string message, string logBy)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Info.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Info, logBy);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LogWarning(string module, int? sessionId, string message, string logBy)
        {
            try
            {
                if (!CheckLogLevelSetting(LogLevels.Warning.ToString()))
                    return;

                var log = LogHandler.CreateLogDto(module, sessionId, message, LogLevels.Warning, logBy);
                SaveLog(log);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Log(string module, int? sessionId, string message, string logLevelKeyword, string logBy)
        {
            try
            {
                if (!CheckLogLevelSetting(logLevelKeyword))
                    return;

                var logLevelId = logLevelRepository.GetByKeyword(logLevelKeyword).Id;
                var log = LogHandler.CreateLogDto(module, sessionId, message, logBy: logBy);
                log.LogLevelKeyword = logLevelKeyword;

                LogHandler.ValidateLog(log);

                logRepository.Add(log.ToEntity(logLevelId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SaveLog(LogDto log, int? logLevelId = null)
        {
            if (!logLevelId.HasValue)
                logLevelId = logLevelRepository.GetByKeyword(log.LogLevel.ToString()).Id;

            LogHandler.ValidateLog(log);

            logRepository.Add(log.ToEntity(logLevelId.Value));
        }

        private bool CheckLogLevelSetting(string logLevel)
        {
            // If the setting is not configured, db logging is turned off.
            var dbMinLogLevel = LogSettings.DbLogMinLevel;
            if (string.IsNullOrEmpty(dbMinLogLevel))
                return false;

            // If the dbMinLogLevel is not in the databse, work like its not set.
            var dbMinLogLevelEntity = logLevelRepository.GetByKeyword(dbMinLogLevel);
            if (dbMinLogLevelEntity == null)
                return false;

            // If the given log level is not in the database, throw an exception to indicate a configuration issue.
            var dbLogLevel = logLevelRepository.GetByKeyword(logLevel);
            if (dbLogLevel == null)
                throw new KeyNotFoundException($"Log level '{logLevel}' not found in the database.");

            return dbLogLevel.Id >= dbMinLogLevelEntity.Id;
        }
    }
}
