using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal
{
    public class HttpRequestWrapper : HttpRequest
    {
        private readonly HttpContext _context;
        private readonly HttpRequest _request;
        private readonly IHeaderDictionary _headers = new HeaderDictionary();
        private readonly IFormFile _file;
        private readonly Stream _body;

        public HttpRequestWrapper(HttpContext context, HttpRequest request, IFormFile file, Stream body)
        {
            _context = context;
            _request = request;
            _file = file;
            _body = body;

            _headers.Add(HeaderNames.ContentType, file.ContentType);
            _headers.Add(HeaderNames.ContentLength, body.Length.ToString());

            foreach (var header in request.Headers)
            {
                if (!header.Key.Equals(HeaderNames.ContentType, StringComparison.OrdinalIgnoreCase) &&
                    !header.Key.Equals(HeaderNames.ContentLength, StringComparison.OrdinalIgnoreCase))
                {
                    _headers.Add(header);
                }
            }
        }

        public override HttpContext HttpContext => _context;

        public override string Method
        {
            get => _request.Method;
            set => _request.Method = value;
        }

        public override string Scheme
        {
            get => _request.Scheme;
            set => _request.Scheme = value;
        }

        public override bool IsHttps
        {
            get => _request.IsHttps;
            set => _request.IsHttps = value;
        }

        public override HostString Host
        {
            get => _request.Host;
            set => _request.Host = value;
        }

        public override PathString PathBase
        {
            get => _request.PathBase;
            set => _request.PathBase = value;
        }

        public override PathString Path
        {
            get => _request.Path;
            set => _request.Path = value;
        }

        public override QueryString QueryString
        {
            get => _request.QueryString;
            set => _request.QueryString = value;
        }

        public override IQueryCollection Query
        {
            get => _request.Query;
            set => _request.Query = value;
        }

        public override string Protocol
        {
            get => _request.Protocol;
            set => _request.Protocol = value;
        }

        public override IHeaderDictionary Headers => _headers;

        public override IRequestCookieCollection Cookies
        {
            get => _request.Cookies;
            set => _request.Cookies = value;
        }

        public override long? ContentLength
        {
            get => _body.Length;
            set => throw new InvalidOperationException();
        }

        public override string ContentType
        {
            get => _file.ContentType;
            set => throw new InvalidOperationException();
        }

        public override Stream Body
        {
            get => _body;
            set => throw new InvalidOperationException();
        }

        public override bool HasFormContentType => _request.HasFormContentType;

        public override IFormCollection Form
        {
            get => _request.Form;
            set => _request.Form = value;
        }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _request.ReadFormAsync(cancellationToken);
        }
    }
}
