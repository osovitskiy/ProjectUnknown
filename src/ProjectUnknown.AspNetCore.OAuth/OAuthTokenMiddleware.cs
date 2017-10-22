using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectUnknown.AspNetCore.Extensions;
using ProjectUnknown.Common;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthTokenMiddleware
    {
        private readonly OAuthTokenOptions _options;

        public OAuthTokenMiddleware(OAuthTokenOptions options)
        {
            Ensure.IsNotNull(options, nameof(options));

            _options = options;
        }

        public async Task Invoke(HttpContext context, IOAuthTokenHandler tokenHandler)
        {
            var request = context.Request;

            if (!request.IsHttps && !_options.AllowInsecureHttp)
            {
                await WriteResultAsync(context.Response, OAuthResult.InvalidRequest("HTTPS is required."));
                return;
            }

            if (!request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                await WriteResultAsync(context.Response, OAuthResult.InvalidRequest("Token request should use 'POST' method."));
                return;
            }

            if (request.ContentType == null)
            {
                await WriteResultAsync(context.Response, OAuthResult.InvalidRequest("Token request should specify Content-Type."));
                return;
            }

            if (!request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                await WriteResultAsync(context.Response, OAuthResult.InvalidRequest("Token request should use 'application/x-www-form-urlencoded' Content-Type."));
                return;
            }

            var form = await request.ReadFormAsync();

            if (form == null)
            {
                await WriteResultAsync(context.Response, OAuthResult.InvalidRequest("Token request should use 'application/x-www-form-urlencoded' Content-Type."));
                return;
            }

            foreach (var pair in form)
            {
                if (pair.Value.Count > 1)
                {
                    await WriteResultAsync(context.Response, OAuthResult.InvalidRequest($"The '{pair.Key}' parameter is repeated."));
                    return;
                }
            }

            var result = await tokenHandler.HandleTokenRequestAsync(new OAuthTokenRequest(form));

            await WriteResultAsync(context.Response, result ?? OAuthResult.ServerError("Token request was unhandled."));
        }
        
        private Task WriteResultAsync(HttpResponse response, OAuthResult result)
        {
            response.StatusCode = result.StatusCode;

            if (result.Body != null)
            {
                return response.WriteJsonAsync(result.Body, _options.SerializerSettings);
            }

            return Task.CompletedTask;
        }
    }
}
