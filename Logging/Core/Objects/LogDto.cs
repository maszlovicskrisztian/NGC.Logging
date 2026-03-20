namespace NGC.Logging.Objects
{
    public class LogDto
    {
        public string Module { get; set; }
        public int SessionId { get; set; }
        public string Message { get; set; }
        public LogLevels? LogLevel { get; set; }
        public string LogLevelKeyword { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LogBy { get; set; }
    }
}
