namespace NGC.Logging.Mapping
{
    using NGC.Logging.DataAccess.Models;
    using NGC.Logging.Objects;
    
    internal static class LogMap
    {
        internal static Log ToEntity(this LogDto logDto, int logLevelId)
        {
            if (logDto == null) return null;

            return new Log
            {
                Module = logDto.Module,
                SessionId = logDto.SessionId,
                Message = logDto.Message,
                CreatedAt = logDto.CreatedAt,
                LogLevelId = logLevelId
            };
        }
    }
}
