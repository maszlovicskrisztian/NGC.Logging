namespace NGC.Logging.DataAccess.Repositories
{
    using NGC.Logging.DataAccess.Models;

    internal interface ILogRepository
    {
        void Add(Log log);
    }
}
