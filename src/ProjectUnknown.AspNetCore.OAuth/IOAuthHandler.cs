using System.Threading.Tasks;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public interface IOAuthHandler
    {
        Task<OAuthResult> HandleTokenRequestAsync(OAuthTokenRequest request);
    }
}
