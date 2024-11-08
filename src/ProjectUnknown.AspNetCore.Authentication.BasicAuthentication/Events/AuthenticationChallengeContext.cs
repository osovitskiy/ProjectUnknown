using System;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Events
{
    public class AuthenticationChallengeContext : PropertiesContext<BasicAuthenticationOptions>
    {
        public AuthenticationChallengeContext(HttpContext context, AuthenticationScheme scheme,
            BasicAuthenticationOptions options, AuthenticationProperties properties)
            : base(context, scheme, options, properties)
        {
        }

        public Exception AuthenticateFailure { get; set; }

        public bool Handled { get; private set; }

        public void HandleResponse() => Handled = true;
    }
}
