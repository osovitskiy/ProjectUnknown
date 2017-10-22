using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public class DefaultJsonWriterFactory : IJsonWriterFactory
    {
        private const int DefaultBufferSize = 16 * 1024;
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public Newtonsoft.Json.JsonWriter Create(Stream stream)
        {
            return new JsonTextWriter(new StreamWriter(stream, DefaultEncoding, DefaultBufferSize, true));
        }
    }
}
