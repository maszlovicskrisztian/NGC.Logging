namespace NGC.Logging.DataAccess.Repositories
{
    using NGC.Logging.DataAccess.Models;

    internal interface ILogLevelRepository
    {
        LogLevel GetByKeyword(string keyword);
    }
}
