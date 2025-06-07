using Newtonsoft.Json;

public class ApplicationErrorModel
{

    [JsonProperty(PropertyName = "source")]
    public string Source { get; set; }

    [JsonProperty(PropertyName = "propertyName")]
    public string PropertyName { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }
}