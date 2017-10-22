using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProjectUnknown.Common;

#if NET6_0
using System.Text.Json;
#endif

namespace ProjectUnknown.AspNetCore.Extensions
{
    public static class HttpResponseExtensions
    {
        private const int DefaultBufferSize = 16 * 1024;
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public static void WriteJson(this HttpResponse response, object value,
            JsonSerializerSettings serializerSettings = null)
        {
            Ensure.IsNotNull(response, nameof(response));
            Ensure.IsNotNull(value, nameof(value));

            WriteJsonInternal(response, value, serializerSettings);
        }

        public static Task WriteJsonAsync(this HttpResponse response, object value,
            JsonSerializerSettings serializerSettings = null)
        {
            Ensure.IsNotNull(response, nameof(response));
            Ensure.IsNotNull(value, nameof(value));

            WriteJsonInternal(response, value, serializerSettings);

            return Task.CompletedTask;
        }

        private static void WriteJsonInternal(HttpResponse response, object value,
            JsonSerializerSettings serializerSettings)
        {
            JsonTextWriter CreateJsonWriter(Stream stream, IServiceProvider services)
            {
                if (services != null)
                {
                    var bytePool = services.GetService<ArrayPool<byte>>();
                    var charPool = services.GetService<ArrayPool<char>>();

                    if (bytePool != null && charPool != null)
                    {
                        return new JsonTextWriter(new HttpResponseStreamWriter(stream, DefaultEncoding,
                            DefaultBufferSize, bytePool, charPool)) {ArrayPool = new JsonArrayPool<char>(charPool)};
                    }
                }

                return new JsonTextWriter(new StreamWriter(stream, DefaultEncoding, DefaultBufferSize, true));
            }

            if (response.ContentType == null)
            {
                response.ContentType = "application/json";
            }

            using (var writer = CreateJsonWriter(response.Body, response.HttpContext.RequestServices))
            {
                Newtonsoft.Json.JsonSerializer.Create(serializerSettings).Serialize(writer, value);
            }
        }

        private class JsonArrayPool<T> : IArrayPool<T>
        {
            private readonly ArrayPool<T> _inner;

            public JsonArrayPool(ArrayPool<T> inner)
            {
                _inner = inner;
            }

            public T[] Rent(int minimumLength)
            {
                return _inner.Rent(minimumLength);
            }

            public void Return(T[] array)
            {
                _inner.Return(array);
            }
        }

#if NET6_0
        public static void WriteJson(this HttpResponse response, object value, JsonSerializerOptions serializerOptions =
 null)
        {
            Ensure.IsNotNull(response, nameof(response));
            Ensure.IsNotNull(value, nameof(value));

            if (response.ContentType == null)
            {
                response.ContentType = "application/json";
            }

            using (var writer = new Utf8JsonWriter(response.Body))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, value, serializerOptions);
            }
        }

        public static Task WriteJsonAsync(this HttpResponse response, object value, JsonSerializerOptions serializerOptions
 = null)
        {
            Ensure.IsNotNull(response, nameof(response));
            Ensure.IsNotNull(value, nameof(value));

            if (response.ContentType == null)
            {
                response.ContentType = "application/json";
            }

            return System.Text.Json.JsonSerializer.SerializeAsync(response.Body, value, serializerOptions);
        }
#endif
    }
}
