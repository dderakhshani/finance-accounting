using System;
using Newtonsoft.Json;

namespace Library
{
    public class SpecialTypesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?)
                                                  || objectType == typeof(DateTime) || objectType == typeof(DateTime?)
                                                  || objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?);
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => reader.Value;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("$type"); // Deserializer helper
            writer.WriteValue(value.GetType().FullName);
            writer.WritePropertyName("$value");
            writer.WriteValue(value);
            writer.WriteEndObject();
        }
    }
}