using Newtonsoft.Json;

namespace Library.Exceptions
{
    public class ApplicationErrorModel
    {

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
