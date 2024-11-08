using System.Threading;
using System.Threading.Tasks;

namespace ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated
{
    public interface IMultipartRelatedFeature
    {
        bool HasMultipartRelatedContentType { get; }
        IMultipartCollection Multipart { get; set; }
        IMultipartCollection ReadMultipart();
        Task<IMultipartCollection> ReadMultipartAsync();
        Task<IMultipartCollection> ReadMultipartAsync(CancellationToken cancellationToken);
    }
}
