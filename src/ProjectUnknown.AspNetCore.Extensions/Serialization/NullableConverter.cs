using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ProjectUnknown.AspNetCore.Extensions.Serialization
{
    public abstract class NullableConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                WriteJsonInternal(writer, value, serializer);
            }
        }

        protected abstract void WriteJsonInternal(JsonWriter writer, object value, JsonSerializer serializer);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                var info = objectType.GetTypeInfo();

                if (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return null;
                }

                throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
            }

            return ReadJsonInternal(reader, objectType, existingValue, serializer);
        }

        protected abstract object ReadJsonInternal(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

        public override bool CanConvert(Type objectType)
        {
            var info = objectType.GetTypeInfo();

            if (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return CanConvertInternal(Nullable.GetUnderlyingType(objectType));
            }
            else
            {
                return CanConvertInternal(objectType);
            }
        }

        protected abstract bool CanConvertInternal(Type objectType);
    }
}
