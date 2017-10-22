using System;
using Microsoft.Extensions.Logging;

namespace ProjectUnknown.AspNetCore.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogException(ILogger logger, Exception exception)
        {
            logger.LogError(default(EventId), exception.Message, exception);
        }
    }
}
