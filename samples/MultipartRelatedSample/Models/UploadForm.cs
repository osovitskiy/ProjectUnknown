using System.ComponentModel.DataAnnotations;

namespace MultipartRelatedSample.Models
{
    public class UploadForm
    {
        [Required]
        public string Cid { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string[] Tags { get; set; }
    }
}
