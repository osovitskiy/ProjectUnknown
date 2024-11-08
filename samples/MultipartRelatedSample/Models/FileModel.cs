namespace MultipartRelatedSample.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
