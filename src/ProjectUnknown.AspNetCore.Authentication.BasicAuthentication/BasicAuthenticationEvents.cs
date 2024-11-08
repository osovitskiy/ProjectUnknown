using System;
using System.Threading.Tasks;

using ProjectUnknown.AspNetCore.Authentication.BasicAuthentication.Events;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public class BasicAuthenticationEvents
    {
        public Func<AuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;
        public Func<AuthenticationSucceededContext, Task> OnAuthenticationSucceeded { get; set; } = context => Task.CompletedTask;
        public Func<AuthenticationChallengeContext, Task> OnChallenge { get; set; } = context => Task.CompletedTask;

        public virtual Task AuthenticationFailed(AuthenticationFailedContext context) => OnAuthenticationFailed(context);
        public virtual Task AuthenticationSucceeded(AuthenticationSucceededContext context) => OnAuthenticationSucceeded(context);
        public virtual Task Challenge(AuthenticationChallengeContext context) => OnChallenge(context);
    }
}
