using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public static class HttpRequestExtensions
    {
        public static bool HasMultipartRelatedContentType(this HttpRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));

            return request.ContentType != null && request.ContentType.StartsWith("multipart/related", StringComparison.OrdinalIgnoreCase);
        }

        public static IMultipartCollection ReadMultipart(this HttpRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));

            var feature = request.HttpContext.Features.Get<IMultipartRelatedFeature>();

            if (feature == null)
            {
                throw new InvalidOperationException();
            }

            return feature.Multipart;
        }

        public static Task<IMultipartCollection> ReadMultipartAsync(this HttpRequest request)
        {
            Ensure.IsNotNull(request, nameof(request));

            var feature = request.HttpContext.Features.Get<IMultipartRelatedFeature>();

            if (feature == null)
            {
                throw new InvalidOperationException();
            }

            return feature.ReadMultipartAsync();
        }

        public static Task<IMultipartCollection> ReadMultipartAsync(this HttpRequest request, CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(request, nameof(request));

            var feature = request.HttpContext.Features.Get<IMultipartRelatedFeature>();

            if (feature == null)
            {
                throw new InvalidOperationException();
            }

            return feature.ReadMultipartAsync(cancellationToken);
        }
    }
}
