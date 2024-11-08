using System;
using System.Linq;
using Microsoft.Net.Http.Headers;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal
{
    internal static class MediaTypeHeaderValues
    {
        public static readonly MediaTypeHeaderValue MultipartRelated = MediaTypeHeaderValue.Parse("multipart/related").CopyAsReadOnly();

        public static string GetInnerType(this MediaTypeHeaderValue value)
        {
            var parameter = value.Parameters.FirstOrDefault(x => x.Name.Equals("type", StringComparison.OrdinalIgnoreCase));

            return parameter != null ? HeaderUtilities.RemoveQuotes(parameter.Value).ToString() : null;
        }

        public static string GetStart(this MediaTypeHeaderValue value)
        {
            var parameter = value.Parameters.FirstOrDefault(x => x.Name.Equals("start", StringComparison.OrdinalIgnoreCase));

            return parameter != null ? HeaderUtilities.RemoveQuotes(parameter.Value).ToString() : null;
        }
    }
}
