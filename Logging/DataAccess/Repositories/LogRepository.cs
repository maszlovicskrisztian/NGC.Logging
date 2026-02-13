namespace NGC.Logging.DataAccess.Repositories
{
    using NGC.Logging.DataAccess.Context;
    using NGC.Logging.DataAccess.Models;

    internal class LogRepository : ILogRepository
    {
        private readonly LoggingDbContext _context;

        public LogRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public void Add(Log log)
        {
            if (log.CreatedAt == default)
            {
                log.CreatedAt = DateTime.UtcNow;
            }
            _context.Logs.Add(log);
            _context.SaveChanges();
        }
    }
}
