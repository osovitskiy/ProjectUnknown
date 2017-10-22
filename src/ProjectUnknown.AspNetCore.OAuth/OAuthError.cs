using Newtonsoft.Json;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorDescription { get; set; }

        [JsonProperty("error_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorUri { get; set; }
    }
}
