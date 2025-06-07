using TaxCollectData.Library.Dto.Transfer;

namespace TaxCollectData.Library.Abstraction;

public interface IEncryptor
{
    List<PacketDto<string>> Encrypt<T>(List<PacketDto<T>> packets);

    PacketDto<string> Encrypt<T>(PacketDto<T> packet);
}