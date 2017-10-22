using System.Threading.Tasks;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public interface IOAuthTokenHandler
    {
        Task<OAuthResult> HandleTokenRequestAsync(OAuthTokenRequest request);
    }
}
