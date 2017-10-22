using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectUnknown.AspNetCore.Utils;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public class JsonObjectWriter : IJsonObjectWriter
    {
        private readonly IJsonWriterFactory factory;
        private readonly JsonObjectWriterOptions options;

        public JsonObjectWriter(IJsonWriterFactory factory, IOptions<JsonObjectWriterOptions> options)
        {
            Ensure.IsNotNull(factory, nameof(factory));
            Ensure.IsNotNull(options, nameof(options));

            this.factory = factory;
            this.options = options.Value;
        }

        public void WriteObject(Stream stream, object value)
        {
            Ensure.IsNotNull(stream, nameof(stream));
            Ensure.IsNotNull(value, nameof(value));

            using (var writer = factory.Create(stream))
            {
                JsonSerializer.Create(options.SerializerSettings).Serialize(writer, value);
            }
        }
    }
}
