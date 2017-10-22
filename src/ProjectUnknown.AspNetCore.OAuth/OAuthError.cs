#if NET6_0
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthError
    {
#if NET6_0
        [JsonPropertyName("error")]
#else
        [JsonProperty("error")]
#endif
        public string Error { get; set; }

#if NET6_0
        [JsonPropertyName("error_description")]
#else
        [JsonProperty("error_description", NullValueHandling = NullValueHandling.Ignore)]
#endif
        public string ErrorDescription { get; set; }

#if NET6_0
        [JsonPropertyName("error_uri")]
#else
        [JsonProperty("error_uri", NullValueHandling = NullValueHandling.Ignore)]
#endif
        public string ErrorUri { get; set; }
    }
}
