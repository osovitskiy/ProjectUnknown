using Microsoft.AspNetCore.Authentication;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
        public BasicAuthenticationOptions()
        {
            Events = new BasicAuthenticationEvents();
        }

        public string Realm { get; set; } = BasicAuthenticationDefaults.Realm;

        public new BasicAuthenticationEvents Events
        {
            get => (BasicAuthenticationEvents) base.Events;
            set => base.Events = value;
        }
    }
}
