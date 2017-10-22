#if NET6_0
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthTokenOptions
    {
        public bool AllowInsecureHttp { get; set; }

#if NET6_0
        public JsonSerializerOptions SerializerSettings { get; set; } =  new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };
#else
        public JsonSerializerSettings SerializerSettings { get; set; }
#endif
    }
}
