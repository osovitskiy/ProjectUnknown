using System.IO;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    public interface IJsonObjectWriter
    {
        void WriteObject(Stream stream, object value);
    }
}
