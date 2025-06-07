using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Dto.Transfer;
using TaxCollectData.Library.Exceptions;

namespace TaxCollectData.Library.Business
{
    internal class DefaultEncryptor : IEncryptor
    {
        private readonly IPacketCodec _packetCodec;
        private readonly EncryptionConfig _encryptionConfig;

        public DefaultEncryptor(IPacketCodec packetCodec, EncryptionConfig encryptionConfig)
        {
            _packetCodec = packetCodec;
            _encryptionConfig = encryptionConfig;
        }

        public List<PacketDto<string>> Encrypt<T>(List<PacketDto<T>> packets)
        {
            var aesKey = _packetCodec.GenerateAesSecretKey();
            var iv = _packetCodec.GenerateIv();
            var stringToBeEncrypted = BitConverter.ToString(aesKey).Replace("-", "");
            var rsaEncryptSymmetricKey = EncryptData(stringToBeEncrypted, _encryptionConfig.TaxOrgPublicKey);

            try
            {
                return packets.Select(packetDto => GetEncryptedPacket(packetDto, aesKey, iv, rsaEncryptSymmetricKey)).ToList();
            }
            catch (Exception e)
            {
                throw new TaxApiException("unable to encrypt invoice json using AES symmetric key", e);
            }
        }

        public PacketDto<string> Encrypt<T>(PacketDto<T> packet)
        {
            var aesKey = _packetCodec.GenerateAesSecretKey();
            var iv = _packetCodec.GenerateIv();
            var stringToBeEncrypted = BitConverter.ToString(aesKey).Replace("-", "");
            var rsaEncryptSymmetricKey = EncryptData(stringToBeEncrypted, _encryptionConfig.TaxOrgPublicKey);

            try
            {
                return GetEncryptedPacket(packet, aesKey, iv, rsaEncryptSymmetricKey);
            }
            catch (Exception e)
            {
                throw new TaxApiException("unable to encrypt invoice json using AES symmetric key", e);
            }
        }

        private PacketDto<string> GetEncryptedPacket<T>(PacketDto<T> packetDto,
            byte[] aesKey,
            byte[] iv,
            string rsaEncryptSymmetricKey)
        {
            return new PacketDto<string>(packetDto.Uid,
                packetDto.PacketType,
                packetDto.FiscalId,
                GetAesEncrypt(packetDto, aesKey, iv),
                packetDto.Retry,
                _encryptionConfig.EncryptionKeyId,
                rsaEncryptSymmetricKey,
                BitConverter.ToString(iv).Replace("-", ""),
                packetDto.DataSignature,
                packetDto.SignatureKeyId);
        }

        private string GetAesEncrypt<T>(PacketDto<T> packetDto, byte[] aesKey, byte[] iv)
        {
            var encryptedData = JsonSerializer.SerializeToUtf8Bytes(packetDto.Data, JsonSerializerConfig.JsonSerializerOptions);
            return AesEncrypt(_packetCodec.Xor(encryptedData, aesKey), aesKey, iv);
        }

        private string AesEncrypt(byte[] payload, byte[] key, byte[] iv)
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            var baPayload = Array.Empty<byte>();
            cipher.Init(true, new AeadParameters(new KeyParameter(key), 128, iv, baPayload));
            var cipherBytes = new byte[cipher.GetOutputSize(payload.Length)];
            var len = cipher.ProcessBytes(payload, 0, payload.Length, cipherBytes, 0);
            cipher.DoFinal(cipherBytes, len);
            return Convert.ToBase64String(cipherBytes);
        }

        private string EncryptData(string stringToBeEncrypted, string publicKey)
        {
            try
            {
                var asymmetricKeyParameter = PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
                var rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
                var rsaParameters = new RSAParameters
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                };
                var rsa = new RSACng();
                rsa.ImportParameters(rsaParameters);
                return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(stringToBeEncrypted),
                    RSAEncryptionPadding.OaepSHA256));
            }
            catch (Exception e)
            {
                return "error";
            }
        }
    }
}