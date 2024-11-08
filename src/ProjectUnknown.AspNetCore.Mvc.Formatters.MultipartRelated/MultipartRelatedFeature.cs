using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated.Internal;

#if !NET6_0
using Microsoft.AspNetCore.Http.Internal;
#endif

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public class MultipartRelatedFeature : IMultipartRelatedFeature
    {
        private static readonly MultipartOptions DefaultMultipartOptions = new MultipartOptions();

        private readonly HttpRequest _request;
        private readonly MultipartOptions _options;
        private IMultipartCollection _parts;

        public MultipartRelatedFeature(HttpRequest request)
            : this(request, DefaultMultipartOptions)
        {
        }

        public MultipartRelatedFeature(HttpRequest request, MultipartOptions options)
        {
            _request = request;
            _options = options;
        }

        public bool HasMultipartRelatedContentType
        {
            get
            {
                if (Multipart != null)
                {
                    return true;
                }

                var contentType = GetContentType();

                return contentType != null && contentType.MediaType.Equals("multipart/related", StringComparison.OrdinalIgnoreCase);
            }
        }

        public IMultipartCollection Multipart
        {
            get => _parts;
            set => _parts = value;
        }

        public IMultipartCollection ReadMultipart()
        {
            if (Multipart != null)
            {
                return Multipart;
            }

            if (!HasMultipartRelatedContentType)
            {
                throw new InvalidOperationException("Incorrect Content-Type: " + _request.ContentType);
            }

            // TODO: Avoid Sync-over-Async http://blogs.msdn.com/b/pfxteam/archive/2012/04/13/10293638.aspx
            // TODO: How do we prevent thread exhaustion?
            return ReadMultipartAsync().GetAwaiter().GetResult();
        }

        public Task<IMultipartCollection> ReadMultipartAsync() => ReadMultipartAsync(CancellationToken.None);

        public async Task<IMultipartCollection> ReadMultipartAsync(CancellationToken cancellationToken)
        {
            if (Multipart != null)
            {
                return Multipart;
            }

            if (!HasMultipartRelatedContentType)
            {
                throw new InvalidOperationException("Incorrect Content-Type: " + _request.ContentType);
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (_options.BufferBody)
            {
                BufferingHelper.EnableRewind(_request, _options.MemoryBufferThreshold, _options.BufferBodyLengthLimit);
            }

            FormFileCollection collection = null;

            var contentType = GetContentType();

            // Some of these code paths use StreamReader which does not support cancellation tokens.
            using (cancellationToken.Register(state => ((HttpContext) state).Abort(), _request.HttpContext))
            {
                var boundary = GetBoundary(contentType, _options.MultipartBoundaryLengthLimit);

                var reader = new MultipartReader(boundary, _request.Body)
                {
                    HeadersCountLimit = _options.MultipartHeadersCountLimit,
                    HeadersLengthLimit = _options.MultipartHeadersLengthLimit,
                    BodyLengthLimit = _options.MultipartBodyLengthLimit,
                };

                while (true)
                {
                    var section = await reader.ReadNextSectionAsync(cancellationToken);

                    if (section == null)
                    {
                        break;
                    }

                    if (section.Headers.TryGetValue("Content-ID", out var contentId))
                    {
                        var cid = (string) contentId;

                        if (cid[0] == '<' && cid[cid.Length - 1] == '>')
                        {
                            cid = cid.Substring(1, cid.Length - 2);
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid Content-ID header value: " + cid);
                        }

                        // Enable buffering for the file if not already done for the full body
                        BufferingHelper.EnableRewind(section, _request.HttpContext.Response.RegisterForDispose, _options.MemoryBufferThreshold, _options.MultipartBodyLengthLimit);

                        // Find the end
                        await section.Body.DrainAsync(cancellationToken);

                        var part = section.BaseStreamOffset.HasValue
                            ? new FormFile(_request.Body, section.BaseStreamOffset.Value, section.Body.Length, cid, null)
                            : new FormFile(section.Body, 0, section.Body.Length, cid, null);

                        part.Headers = new HeaderDictionary(section.Headers);

                        if (collection == null)
                        {
                            collection = new FormFileCollection();
                        }

                        if (collection.Count >= _options.PartsCountLimit)
                        {
                            throw new InvalidDataException($"Form value count limit {_options.PartsCountLimit} exceeded.");
                        }

                        collection.Add(part);
                    }
                }
            }

            // Rewind so later readers don't have to.
            if (_request.Body.CanSeek)
            {
                _request.Body.Seek(0, SeekOrigin.Begin);
            }

            if (collection == null)
            {
                throw new InvalidDataException("Multipart/Related root part is missing.");
            }

            var innerType = contentType.GetInnerType();
            var start = contentType.GetStart();

            IFormFile root;

            if (start == null)
            {
                root = collection.First();
            }
            else
            {
                root = collection.GetFile(start);

                if (root == null)
                {
                    throw new InvalidDataException("Multipart/Related root part is missing.");
                }
            }

            if (!root.ContentType.StartsWith(innerType, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException($"Multipart/Related Content-Type '{innerType}' is disctinct from root part '{root.ContentType}'.");
            }

            Multipart = new MultipartCollection(collection, innerType, start);

            return Multipart;
        }

        private MediaTypeHeaderValue GetContentType()
        {
            MediaTypeHeaderValue contentType;
            MediaTypeHeaderValue.TryParse(_request.ContentType, out contentType);

            return contentType;
        }

        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        private static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);

            if (StringSegment.IsNullOrEmpty(contentType.Boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.ToString();
        }
    }
}
