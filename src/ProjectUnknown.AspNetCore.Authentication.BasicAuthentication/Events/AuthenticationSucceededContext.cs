using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Events
{
    public class AuthenticationSucceededContext : ResultContext<BasicAuthenticationOptions>
    {
        public string UserName { get; }

        public AuthenticationSucceededContext(HttpContext context, AuthenticationScheme scheme,
            BasicAuthenticationOptions options, string userName) : base(context, scheme, options)
        {
            UserName = userName;
        }
    }
}
