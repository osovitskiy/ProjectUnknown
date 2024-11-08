using System.Threading.Tasks;

namespace ProjectUnknown.AspNetCore.Authentication.BasicAuthentication
{
    public interface ICredentialsValidator
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}
