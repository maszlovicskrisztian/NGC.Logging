namespace NGC.Logging.DataAccess.Models
{
    internal class Log
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public int SessionId { get; set; }
        public string Message { get; set; }
        public int LogLevelId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation property
        public LogLevel? LogLevel { get; set; }
    }
}
