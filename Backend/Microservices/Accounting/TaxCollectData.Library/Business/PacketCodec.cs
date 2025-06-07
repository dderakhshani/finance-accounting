using System.Security.Cryptography;
using TaxCollectData.Library.Abstraction;

namespace TaxCollectData.Library.Business;

internal class PacketCodec : IPacketCodec
{
    public byte[] GenerateAesSecretKey()
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        return aes.Key;
    }

    public byte[] GenerateIv()
    {
        var rnd = new RNGCryptoServiceProvider();
        var b = new byte[16];
        rnd.GetNonZeroBytes(b);
        return b;
    }

    public byte[] Xor(byte[] b1, byte[] b2)
    {
        return b1.Length < b2.Length ? XorBlocks(b1, b2) : XorBlocks(b2, b1);
    }

    private byte[] XorBlocks(byte[] smallerArray, byte[] biggerArray)
    {
        var oneAndTwo = new byte[biggerArray.Length];
        var blockSize = (int)Math.Ceiling((double)biggerArray.Length / smallerArray.Length);
        for (var i = 0; i < blockSize; i++)
        {
            for (var j = 0; j < smallerArray.Length; j++)
            {
                if (i * smallerArray.Length + j >= biggerArray.Length)
                {
                    break;
                }

                oneAndTwo[i * smallerArray.Length + j] = (byte)(smallerArray[j] ^ biggerArray[i * smallerArray.Length + j]);
            }
        }

        return oneAndTwo;
    }
}