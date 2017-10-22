using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthTokenRequest : OAuthRequest
    {
        public OAuthTokenRequest(IFormCollection form)
            : base(form)
        {
        }

        public string GrantType => GetParam(OAuthConstants.ParamNames.GrantType);
        public string Username => GetParam(OAuthConstants.ParamNames.Username);
        public string Password => GetParam(OAuthConstants.ParamNames.Password);
        public string RefreshToken => GetParam(OAuthConstants.ParamNames.RefreshToken);

        public bool IsPasswordRequest()
        {
            return GrantType == OAuthConstants.GrantTypes.Password;
        }

        public bool IsRefreshTokenRequest()
        {
            return GrantType == OAuthConstants.GrantTypes.RefreshToken;
        }
    }
}
