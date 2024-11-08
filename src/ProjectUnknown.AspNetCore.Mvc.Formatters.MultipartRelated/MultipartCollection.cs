using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public class MultipartCollection : IMultipartCollection
    {
        private readonly IFormFileCollection _parts;
        private readonly string _contentType;
        private readonly string _start;

        public MultipartCollection(IFormFileCollection parts, string contentType, string start)
        {
            _parts = parts;
            _contentType = contentType;
            _start = start;
        }

        public string ContentType => _contentType;

        public IFormFileCollection Parts => _parts;

        public IFormFile Root
        {
            get
            {
                if (_start != null)
                {
                    return GetPart(_start);
                }
                else
                {
                    return _parts.FirstOrDefault();
                }
            }
        }

        public IFormFile GetPart(string id)
        {
            return _parts.GetFile(id);
        }

        public IFormFile GetPart(string id, string type)
        {
            return _parts.GetFiles(id).FirstOrDefault(file => file.ContentType.StartsWith(type, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<IFormFile> GetParts(string id)
        {
            return _parts.GetFiles(id);
        }
    }
}
