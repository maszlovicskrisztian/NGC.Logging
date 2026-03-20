namespace NGC.Logging.Core
{
    internal class CompositeLogger : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;

        internal CompositeLogger(IEnumerable<ILogger> loggers)
        {
            _loggers = loggers ?? throw new ArgumentNullException(nameof(loggers));
        }

        public void LogInfo(string module, int? sessionId, string message, string logBy)
            => Dispatch(l => l.LogInfo(module, sessionId, message, logBy));

        public void LogWarning(string module, int? sessionId, string message, string logBy)
            => Dispatch(l => l.LogWarning(module, sessionId, message, logBy));

        public void LogError(string module, int? sessionId, string message, string logBy)
            => Dispatch(l => l.LogError(module, sessionId, message, logBy));

        public void Log(string module, int? sessionId, string message, string logLevelKeyword, string logBy)
            => Dispatch(l => l.Log(module, sessionId, message, logLevelKeyword, logBy));

        private void Dispatch(Action<ILogger> logAction)
        {
            List<Exception>? exceptions = null;

            foreach (var logger in _loggers)
            {
                try
                {
                    logAction(logger);
                }
                catch (Exception ex)
                {
                    exceptions ??= [];
                    exceptions.Add(ex);
                }
            }

            if (exceptions?.Count == 1)
                throw exceptions[0];

            if (exceptions?.Count > 1)
                throw new AggregateException("One or more loggers failed.", exceptions);
        }
    }
}
