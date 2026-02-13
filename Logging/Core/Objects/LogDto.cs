namespace NGC.Logging.Objects
{
    internal class LogDto
    {
        internal string Module { get; set; }
        internal int SessionId { get; set; }
        internal string Message { get; set; }
        internal LogLevels? LogLevel { get; set; }
        internal string LogLevelKeyword { get; set; }
        internal DateTime CreatedAt { get; set; }
    }
}
