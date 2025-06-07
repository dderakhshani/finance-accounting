using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Common.Common.Attributes
{
    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<T>);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                return serializer.Deserialize<List<T>>(reader);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var item = serializer.Deserialize<T>(reader);
                return new List<T> { item };
            }
            return null;
        }
    }

}
