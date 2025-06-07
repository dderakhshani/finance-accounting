namespace TaxCollectData.Library.Abstraction;

public interface IPacketCodec
{
    byte[] GenerateAesSecretKey();
    byte[] GenerateIv();
    byte[] Xor(byte[] b1, byte[] b2);
}