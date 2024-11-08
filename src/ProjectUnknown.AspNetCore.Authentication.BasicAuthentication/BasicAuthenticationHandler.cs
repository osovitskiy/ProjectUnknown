using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

using ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Events;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly ICredentialsValidator _credentialsValidator;

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, ICredentialsValidator credentialsValidator) : base(options, logger,
            encoder, clock)
        {
            _credentialsValidator = credentialsValidator;
        }

        protected new BasicAuthenticationEvents Events {
            get => (BasicAuthenticationEvents)base.Events;
            set => base.Events = value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try {
                if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var values)) {
                    return AuthenticateResult.NoResult();
                }

                if (!AuthenticationHeaderValue.TryParse(values, out var header)) {
                    return AuthenticateResult.NoResult();
                }

                if (!header.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase)) {
                    return AuthenticateResult.NoResult();
                }

                string credentials;
                try {
                    credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter));
                }
                catch (Exception) {
                    return AuthenticateResult.Fail("The credentials can not be decoded in 'Basic' scheme.");
                }

                var delimiterIdx = credentials.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                if (delimiterIdx < 0) {
                    return AuthenticateResult.Fail("The credentials delimiter is not present in 'Basic' scheme.");
                }

                var username = credentials.Substring(0, delimiterIdx);
                var password = credentials.Substring(delimiterIdx + 1);
                if (!await _credentialsValidator.ValidateCredentialsAsync(username, password)) {
                    return AuthenticateResult.Fail("Authentication failed.");
                }

                var claims = new[] {new Claim(ClaimTypes.Name, username, ClaimValueTypes.String, ClaimsIssuer)};

                var succeededContext =
                    new AuthenticationSucceededContext(Context, Scheme, Options, username) {
                        Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name))
                    };
                await Events.AuthenticationSucceeded(succeededContext);

                if (succeededContext.Result != null) {
                    return succeededContext.Result;
                }

                succeededContext.Success();
                return succeededContext.Result;
            }
            catch (Exception e) {
                Logger.LogError(e, "Exception occurred while processing message");

                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options, e);
                await Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.Result != null) {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authenticateResult = await HandleAuthenticateOnceSafeAsync();

            var challengeContext =
                new AuthenticationChallengeContext(Context, Scheme, Options, properties) {
                    AuthenticateFailure = authenticateResult?.Failure
                };
            await Events.Challenge(challengeContext);

            if (challengeContext.Handled) {
                return;
            }

            Response.StatusCode = 401;
            Response.Headers.Append(HeaderNames.WWWAuthenticate,
                $"{BasicAuthenticationDefaults.AuthenticationScheme} realm=\"{Options.Realm}\"");
        }
    }
}
