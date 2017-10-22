using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProjectUnknown.AspNetCore.JsonWriter;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly OAuthOptions options;

        public OAuthMiddleware(RequestDelegate next, IOptions<OAuthOptions> options)
        {
            Ensure.IsNotNull(next, nameof(next));
            Ensure.IsNotNull(options, nameof(options));

            this.next = next;
            this.options = options.Value;
        }

        public async Task Invoke(HttpContext context, IOAuthHandler handler, IJsonObjectWriter objectWriter)
        {
            Ensure.IsNotNull(context, nameof(context));
            Ensure.IsNotNull(handler, nameof(handler));
            Ensure.IsNotNull(objectWriter, nameof(objectWriter));

            if (context.Request.Path == options.TokenEndpointPath)
            {
                if (!context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
                {
                    WriteResult(context, objectWriter, OAuthResult.InvalidRequest("Token request should use 'POST' method."));
                    return;
                }

                if (context.Request.ContentType == null)
                {
                    WriteResult(context, objectWriter, OAuthResult.InvalidRequest("Token request should specify Content-Type."));
                    return;
                }

                if (!context.Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                {
                    WriteResult(context, objectWriter, OAuthResult.InvalidRequest("Token request should use 'application/x-www-form-urlencoded' Content-Type."));
                    return;
                }

                var form = await context.Request.ReadFormAsync();

                if (form == null)
                {
                    WriteResult(context, objectWriter, OAuthResult.InvalidRequest("Token request should use 'application/x-www-form-urlencoded' Content-Type."));
                    return;
                }

                foreach (var pair in form)
                {
                    if (pair.Value.Count > 1)
                    {
                        WriteResult(context, objectWriter, OAuthResult.InvalidRequest($"The '{pair.Key}' parameter is repeated."));
                        return;
                    }
                }

                var result = await handler.HandleTokenRequestAsync(new OAuthTokenRequest(form));

                if (result != null)
                {
                    WriteResult(context, objectWriter, result);
                }
                else
                {
                    WriteResult(context, objectWriter, OAuthResult.ServerError("Token request was unhandled."));
                }
            }
            else
            {
                await next(context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteResult(HttpContext context, IJsonObjectWriter objectWriter, OAuthResult result)
        {
            var response = context.Response;

            response.StatusCode = result.StatusCode;

            if (result.Body != null)
            {
                response.ContentType = "application/json";

                objectWriter.WriteObject(response.Body, result.Body);
            }
        }
    }
}
