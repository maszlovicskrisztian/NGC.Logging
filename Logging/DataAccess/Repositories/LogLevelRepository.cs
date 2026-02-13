namespace NGC.Logging.DataAccess.Repositories
{
    using NGC.Logging.DataAccess.Context;
    using NGC.Logging.DataAccess.Models;

    internal class LogLevelRepository : ILogLevelRepository
    {
        private readonly LoggingDbContext _context;

        public LogLevelRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public LogLevel GetByKeyword(string keyword)
        {
            return _context.LogLevels
                .SingleOrDefault(ll => ll.Keyword.ToLower() == keyword.ToLower());
        }
    }
}
