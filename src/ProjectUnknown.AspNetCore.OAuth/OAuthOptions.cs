using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthOptions
    {
        public bool AllowInsecureHttp { get; set; }
        public PathString TokenEndpointPath { get; set; }
    }
}
