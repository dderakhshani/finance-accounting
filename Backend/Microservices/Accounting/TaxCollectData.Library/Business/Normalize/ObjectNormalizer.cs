using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using System.Text;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Dto;

[assembly: InternalsVisibleTo("TaxCollectData.Test")]
[assembly: InternalsVisibleTo("TaxCollectData.Sample")]
namespace TaxCollectData.Library.Business.Normalize;

internal class ObjectNormalizer : INormalizer
{
    private readonly string _tokenHeaderName;
    private readonly Dictionary<string, string> _customHeaders;

    public ObjectNormalizer(Dictionary<string, string> customHeaders, string tokenHeaderName = TransferConstants.AuthorizationHeader)
    {
        _customHeaders = customHeaders;
        _tokenHeaderName = tokenHeaderName;
    }

    public string Normalize<T>(T data, Dictionary<string, string> headers)
    {
        if (data == null && headers == null)
        {
            return null;
        }

        var map = GetToken(data) ?? new JObject();

        return Normalize(headers, map);
    }

    public string NormalizeJson(string data, Dictionary<string, string> headers)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        var map = JObject.Parse(data) ?? new JObject();

        return Normalize(headers, map);
    }

    private string Normalize(Dictionary<string, string> headers, JObject map)
    {
        if (headers != null)
        {
            SetHeaders(headers, map);
        }

        var result = new JObject();
        FlatMap(null, map, result);

        var sb = new StringBuilder();
        var keys = result.Properties().Select(p => p.Name).ToList();
        keys.Sort();
        foreach (var key in keys)
        {
            string textValue;
            var value = result[key];
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                textValue = value.ToString();
                textValue = string.IsNullOrEmpty(textValue) ? "#" : textValue.Replace("#", "##");
            }
            else
            {
                textValue = "#";
            }

            sb.Append(textValue).Append('#');
        }

        return sb.Remove(sb.Length - 1, 1).ToString();
    }

    private static readonly ISet<string> validHeaders = new HashSet<string>()
    {
        TransferConstants.SignatureHeader,
        TransferConstants.SignatureKeyIdHeader,
        TransferConstants.RequestTraceIdHeader,
        TransferConstants.TimestampHeader
    };

    private void SetHeaders(Dictionary<string, string> headers, JObject map)
    {
        var signingHeaders = headers.Where(x => !_customHeaders.ContainsKey(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
        foreach (var keyValuePair in signingHeaders)
        {
            if (keyValuePair.Key.Equals(_tokenHeaderName) 
                && keyValuePair.Value is
                {
                    Length: > 7
                })
            {
                map.Add(TransferConstants.AuthorizationHeader, keyValuePair.Value.Substring(7));
            }
            else if(validHeaders.Contains(keyValuePair.Key))
            {
                map.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }

    private static JObject? GetToken(object? data)
    {
        return data switch
        {
            null => null,
            IEnumerable<object> enumerable => JObject.FromObject(new PacketsCollectionWrapper(enumerable)),
            _ => JObject.FromObject(data)
        };
    }

    private static string GetKey(string rootKey, string myKey)
    {
        return !string.IsNullOrWhiteSpace(rootKey) ? $"{rootKey}.{myKey}" : myKey;
    }

    private static void FlatMap(string rootKey, object? input, JObject result)
    {
        switch (input)
        {
            case JArray list:
            {
                foreach (var e in list.Select((x, i) => (x, i)))
                {
                    var key = GetKey(rootKey, "E" + e.i);
                    FlatMap(key, e.x, result);
                }

                return;
            }
            case JObject map:
            {
                foreach (var keyValuePair in map)
                {
                    FlatMap(GetKey(rootKey, keyValuePair.Key), keyValuePair.Value, result);
                }

                return;
            }
            case JValue value:
            {
                result.Add(rootKey,
                    value.Type == JTokenType.Boolean
                        ? JToken.FromObject(value.ToString().ToLowerInvariant())
                        : JToken.FromObject(value));
                return;
            }
            default:
                result.Add(rootKey, null);
                return;
        }
    }
}