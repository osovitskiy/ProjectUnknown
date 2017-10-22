using System.IO;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public interface IJsonWriterFactory
    {
        Newtonsoft.Json.JsonWriter Create(Stream stream);
    }
}
