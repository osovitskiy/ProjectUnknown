using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultipartRelatedSample.Models;
using ProjectUnknown.AspNetCore.Mvc.Formatters.MultipartRelated;

namespace MultipartRelatedSample.Controllers
{
    [Route("api/files")]
    public class FilesController : Controller
    {
        private static readonly ConcurrentDictionary<string, FileModel> Store = new ConcurrentDictionary<string, FileModel>();

        [HttpGet]
        public ActionResult<List<FileViewModel>> Get()
        {
            return Store.Values.Select(FileViewModel).ToList();
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (Store.TryGetValue(name, out var file))
            {
                return File(file.Content, file.ContentType);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UploadForm form)
        {
            var multipart = await Request.ReadMultipartAsync();
            var part = multipart.GetPart(form.Cid);

            var stream = new MemoryStream();

            await part.CopyToAsync(stream);

            var file = new FileModel
            {
                Name = form.Name,
                Tags = form.Tags,
                ContentType = part.ContentType,
                Content = stream.ToArray()
            };

            if (Store.TryAdd(file.Name, file))
            {
                return CreatedAtAction(nameof(Get), new {name = file.Name}, FileViewModel(file));
            }

            return Conflict();
        }

        private static FileViewModel FileViewModel(FileModel file)
        {
            return new FileViewModel
            {
                Name = file.Name,
                Tags = file.Tags,
                ContentType = file.ContentType,
                Size = file.Content.Length
            };
        }
    }
}
