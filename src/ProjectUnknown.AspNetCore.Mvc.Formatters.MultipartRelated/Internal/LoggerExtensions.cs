using System;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, IInputFormatter, string, Exception> InputFormatterSelectedAction =
            LoggerMessage.Define<IInputFormatter, string>(LogLevel.Debug, 1, "Selected input formatter '{InputFormatter}' for content type '{ContentType}'.");

        private static readonly Action<ILogger, IInputFormatter, string, Exception> InputFormatterRejectedAction =
            LoggerMessage.Define<IInputFormatter, string>(LogLevel.Debug, 2, "Rejected input formatter '{InputFormatter}' for content type '{ContentType}'.");

        private static readonly Action<ILogger, string, Exception> NoInputFormatterSelectedAction =
            LoggerMessage.Define<string>(LogLevel.Debug, 3, "No input formatter was found to support the content type '{ContentType}' for use with the [FromBody] attribute.");

        public static void InputFormatterSelected(this ILogger logger, IInputFormatter inputFormatter, InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                InputFormatterSelectedAction(logger, inputFormatter, contentType, null);
            }
        }

        public static void InputFormatterRejected(this ILogger logger, IInputFormatter inputFormatter, InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                InputFormatterRejectedAction(logger, inputFormatter, contentType, null);
            }
        }

        public static void NoInputFormatterSelected(this ILogger logger, InputFormatterContext formatterContext)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var contentType = formatterContext.HttpContext.Request.ContentType;
                NoInputFormatterSelectedAction(logger, contentType, null);
            }
        }
    }
}
