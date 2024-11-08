using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

#if !NET6_0
using Microsoft.AspNetCore.Http.Authentication;
#endif

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal
{
    internal class HttpContextWrapper : HttpContext
    {
        private readonly HttpContext _context;
        private readonly HttpRequestWrapper _request;

        public HttpContextWrapper(HttpContext context, IFormFile file, Stream body)
        {
            _context = context;
            _request = new HttpRequestWrapper(this, context.Request, file, body);
        }

        public override void Abort()
        {
            _context.Abort();
        }

        public override IFeatureCollection Features => _context.Features;
        public override HttpRequest Request => _request;
        public override HttpResponse Response => _context.Response;
        public override ConnectionInfo Connection => _context.Connection;
        public override WebSocketManager WebSockets => _context.WebSockets;

#if !NET6_0
        [Obsolete]
        public override AuthenticationManager Authentication => _context.Authentication;
#endif

        public override ClaimsPrincipal User
        {
            get => _context.User;
            set => _context.User = value;
        }

        public override IDictionary<object, object> Items
        {
            get => _context.Items;
            set => _context.Items = value;
        }

        public override IServiceProvider RequestServices
        {
            get => _context.RequestServices;
            set => _context.RequestServices = value;
        }

        public override CancellationToken RequestAborted
        {
            get => _context.RequestAborted;
            set => _context.RequestAborted = value;
        }

        public override string TraceIdentifier
        {
            get => _context.TraceIdentifier;
            set => _context.TraceIdentifier = value;
        }

        public override ISession Session
        {
            get => _context.Session;
            set => _context.Session = value;
        }
    }
}
