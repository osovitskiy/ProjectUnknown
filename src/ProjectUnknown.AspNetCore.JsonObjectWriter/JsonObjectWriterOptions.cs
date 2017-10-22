using Newtonsoft.Json;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public class JsonObjectWriterOptions
    {
        public JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings();
    }
}
