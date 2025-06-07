using System;
using Newtonsoft.Json;

namespace Library.Configurations
{
    public class TrimmingStringConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string value)
            {
                return value.Trim();
            }

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            //  writer.WriteValue(value);
            throw new NotImplementedException();
        }
    }
}