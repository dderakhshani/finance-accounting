using TaxCollectData.Library.Dto;
using TaxCollectData.Library.Dto.Transfer;

namespace TaxCollectData.Library.Abstraction;

public interface IPacketDtoAdapter
{
    PacketDtoWithoutSignatureKeyId<T> GetPacketDtoWithoutSignatureKeyId<T>(PacketDto<T> packet);
    List<PacketDtoWithoutSignatureKeyId<T>> GetPacketDtoWithoutSignatureKeyIdList<T>(List<PacketDto<T>> packets);
}