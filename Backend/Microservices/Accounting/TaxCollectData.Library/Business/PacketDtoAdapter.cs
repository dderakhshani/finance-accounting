using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Transfer;

namespace TaxCollectData.Library.Business;

internal class PacketDtoAdapter : IPacketDtoAdapter
{
    public PacketDtoWithoutSignatureKeyId<T> GetPacketDtoWithoutSignatureKeyId<T>(PacketDto<T> packet)
    {
        return new PacketDtoWithoutSignatureKeyId<T>(packet.Uid,
            packet.PacketType,
            packet.Retry,
            packet.Data,
            packet.EncryptionKeyId,
            packet.SymmetricKey,
            packet.Iv,
            packet.FiscalId,
            packet.DataSignature);
    }
    public List<PacketDtoWithoutSignatureKeyId<T>> GetPacketDtoWithoutSignatureKeyIdList<T>(List<PacketDto<T>> packets)
    {
        return packets.Select(GetPacketDtoWithoutSignatureKeyId).ToList();
    }
}