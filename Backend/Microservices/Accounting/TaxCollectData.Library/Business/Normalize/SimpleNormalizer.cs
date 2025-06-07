using System.Text;
using System.Text.Json;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Exceptions;

namespace TaxCollectData.Library.Business.Normalize;

internal class SimpleNormalizer : INormalizer
{
    private readonly string _tokenHeaderName;

    public SimpleNormalizer(string tokenHeaderName = TransferConstants.AuthorizationHeader)
    {
        _tokenHeaderName = tokenHeaderName;
    }

    public string Normalize<T>(T data, Dictionary<string, string> headers)
    {
        var serializeToUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(data, JsonSerializerConfig.JsonSerializerOptions);
        var json = Encoding.UTF8.GetString(serializeToUtf8Bytes);
        return NormalizeJson(json, headers);
    }

    private static readonly ISet<string> validHeaders = new HashSet<string>()
    {
        TransferConstants.SignatureHeader,
        TransferConstants.SignatureKeyIdHeader,
        TransferConstants.RequestTraceIdHeader,
        TransferConstants.TimestampHeader
    };


    public string NormalizeJson(string data, Dictionary<string, string> headers)
    {
        if (string.IsNullOrWhiteSpace(data) || headers is null || !headers.Any())
        {
            return data;
        }

        var normalText = new StringBuilder(data);

        var keys = new List<string>
        {
            _tokenHeaderName,
            TransferConstants.RequestTraceIdHeader,
            TransferConstants.TimestampHeader
        };
        
        foreach (var key in keys)
        {
            if (headers.TryGetValue(key, out var header))
            {
                normalText.Append(header);
            }
        }

        return normalText.ToString();
    }
}