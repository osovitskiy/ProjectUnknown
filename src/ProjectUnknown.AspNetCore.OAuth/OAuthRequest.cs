using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthRequest
    {
        private readonly IFormCollection form;

        public OAuthRequest(IFormCollection form)
        {
            this.form = form;
        }

        public virtual string GetParam(string name)
        {
            if (form.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
