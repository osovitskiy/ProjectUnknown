using System;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Events
{
    public class AuthenticationFailedContext : ResultContext<BasicAuthenticationOptions>
    {
        public Exception Exception { get; }

        public AuthenticationFailedContext(HttpContext context, AuthenticationScheme scheme,
            BasicAuthenticationOptions options, Exception exception) : base(context, scheme, options)
        {
            Exception = exception;
        }
    }
}
