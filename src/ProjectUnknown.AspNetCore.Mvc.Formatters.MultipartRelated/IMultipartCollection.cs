using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public interface IMultipartCollection
    {
        string ContentType { get; }
        IFormFileCollection Parts { get; }
        IFormFile Root { get; }
        IFormFile GetPart(string id);
        IFormFile GetPart(string id, string type);
        IEnumerable<IFormFile> GetParts(string id);
    }
}
