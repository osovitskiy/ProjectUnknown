using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.OAuth
{
    public class OAuthRequest
    {
        private readonly IFormCollection _form;

        public OAuthRequest(IFormCollection form)
        {
            _form = form;
        }

        public virtual string GetParam(string name)
        {
            if (_form.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
