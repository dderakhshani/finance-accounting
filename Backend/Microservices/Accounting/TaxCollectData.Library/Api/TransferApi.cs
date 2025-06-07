using System.Text;
using System.Text.Json;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Constants;
using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Transfer;
using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Api;

public abstract class TransferApi : ITransferApi
{
    private readonly IHttpRequestSender _httpRequestSender;
    private readonly IEncryptor _encryptor;

    internal TransferApi(INormalizer normalizer,
        ITransferSignatory transferSignatory,
        IContentSignatory packetSignatory,
        IHttpRequestSender httpRequestSender,
        IEncryptor encryptor)
    {
        Normalizer = normalizer ?? throw new ArgumentNullException(nameof(normalizer));
        TransferSignatory = transferSignatory ?? throw new ArgumentNullException(nameof(transferSignatory));
        PacketSignatory = packetSignatory;
        _httpRequestSender = httpRequestSender ?? throw new ArgumentNullException(nameof(httpRequestSender));
        _encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
    }

    protected INormalizer Normalizer { get; }

    protected ITransferSignatory TransferSignatory { get; }
    protected IContentSignatory PacketSignatory { get; }

    public async Task<HttpResponse<AsyncResponseModel?>?> SendPacketsAsync<TRequest>(List<PacketDto<TRequest>> packets,
        string url,
        Dictionary<string, string>? headers = null,
        bool encrypt = true,
        bool sign = true)
    {
        if (packets == null)
        {
            throw new ArgumentNullException(nameof(packets));
        }

        if (!packets.Any())
        {
            return null;
        }

        headers ??= new Dictionary<string, string>();
        FillEssentialHeaders(headers);

        var sendData = GetSendData(packets, headers, encrypt, sign);

        return await _httpRequestSender.SendPostRequestAsync<AsyncResponseModel>(url, sendData, headers)
            .ConfigureAwait(false);
    }

    public async Task<HttpResponse<SyncResponseModel<TResponse>?>> SendPacketAsync<TRequest, TResponse>(PacketDto<TRequest> packet,
        string url,
        Dictionary<string, string>? headers = null,
        bool encrypt = true,
        bool sign = true)
    {
        if (packet == null)
        {
            throw new ArgumentNullException(nameof(packet));
        }

        headers ??= new Dictionary<string, string>();
        FillEssentialHeaders(headers);

        var sendData = GetSendData<TRequest, TResponse>(packet, headers, encrypt, sign);

        return await _httpRequestSender.SendPostRequestAsync<SyncResponseModel<TResponse>>(url,sendData, headers)
            .ConfigureAwait(false);
    }

    private string GetSendData<TRequest>(List<PacketDto<TRequest>> packets, Dictionary<string, string> headers, bool encrypt, bool sign)
    {
        var signedPackets = sign ? packets.Select(GetSignedPacket).ToList() : packets;
        if (!encrypt)
        {
            return Serialize(GetAsyncSendData(signedPackets, headers));
        }

        var encryptedPacket = _encryptor.Encrypt(signedPackets);
        return Serialize(GetAsyncSendData(encryptedPacket, headers));
    }

    protected static string Serialize<T>(T request)
    {
        return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerializerConfig.JsonSerializerOptions));
    }

    private string GetSendData<TRequest, TResponse>(PacketDto<TRequest> packet, Dictionary<string, string> headers, bool encrypt, bool sign)
    {
        var signedPacket = sign ? GetSignedPacket(packet) : packet;
        if (!encrypt)
        {
            return Serialize(GetSyncSendData<TRequest>(signedPacket, headers));
        }

        var encryptedPacket = _encryptor.Encrypt(packet);
        return Serialize(GetSyncSendData(encryptedPacket, headers));
    }

    protected abstract SyncRequestDto<T> GetSyncSendData<T>(PacketDto<T> packet, Dictionary<string, string> headers);

    protected abstract AsyncRequestDto<T> GetAsyncSendData<T>(List<PacketDto<T>> packets, Dictionary<string, string> headers);

    private void FillEssentialHeaders(Dictionary<string, string> essentialHeaders)
    {
        if (!essentialHeaders.ContainsKey(TransferConstants.RequestTraceIdHeader))
        {
            essentialHeaders.Add(TransferConstants.RequestTraceIdHeader, Guid.NewGuid().ToString());
        }

        if (essentialHeaders.ContainsKey(TransferConstants.TimestampHeader)) return;
        var now = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        essentialHeaders.Add(TransferConstants.TimestampHeader, now.ToString());
    }

    private PacketDto<T> GetSignedPacket<T>(PacketDto<T> packet)
    {
        var packetNormalize = Normalizer.Normalize(packet.Data, new Dictionary<string, string>());
        var packetSignature = PacketSignatory.Sign(packetNormalize);

        return new PacketDto<T>(packet.Uid,
            packet.PacketType,
            packet.FiscalId,
            packet.Data,
            packet.Retry,
            packet.EncryptionKeyId,
            packet.SymmetricKey,
            packet.Iv,
            packetSignature,
            TransferSignatory.GetKeyId());
    }

    private PacketDto<string> GetSerializedPacket<T>(PacketDto<T> packet)
    {
        return new PacketDto<string>(packet.Uid,
            packet.PacketType,
            packet.FiscalId,
            Serialize(packet.Data),
            packet.Retry,
            packet.EncryptionKeyId,
            packet.SymmetricKey,
            packet.Iv,
            packet.DataSignature,
            packet.SignatureKeyId);
    }
}