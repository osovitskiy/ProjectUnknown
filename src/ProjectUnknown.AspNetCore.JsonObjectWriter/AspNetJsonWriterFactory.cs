using System.Buffers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public class AspNetJsonWriterFactory : IJsonWriterFactory
    {
        private const int DefaultBufferSize = 16 * 1024;
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        private readonly ArrayPool<byte> bytePool;
        private readonly ArrayPool<char> charPool;

        public AspNetJsonWriterFactory(ArrayPool<byte> bytePool, ArrayPool<char> charPool)
        {
            Ensure.IsNotNull(bytePool, nameof(bytePool));
            Ensure.IsNotNull(charPool, nameof(charPool));

            this.bytePool = bytePool;
            this.charPool = charPool;
        }

        public Newtonsoft.Json.JsonWriter Create(Stream stream)
        {
            return new JsonTextWriter(new HttpResponseStreamWriter(stream, DefaultEncoding, DefaultBufferSize, bytePool, charPool))
            {
                ArrayPool = new JsonArrayPool<char>(charPool)
            };
        }
    }
}
