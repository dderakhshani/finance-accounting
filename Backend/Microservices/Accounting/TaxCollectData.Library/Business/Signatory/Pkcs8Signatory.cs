using Org.BouncyCastle.OpenSsl;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Dto.Config;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;

namespace TaxCollectData.Library.Business.Signatory;

internal class Pkcs8Signatory : ISignatory, ITransferSignatory, IContentSignatory
{
    private readonly Pkcs8SignatoryConfig _signatoryConfig;
    private RSACryptoServiceProvider _cryptoServiceProvider;
    private bool _isInitialized;

    public Pkcs8Signatory(Pkcs8SignatoryConfig signatoryConfig)
    {
        _signatoryConfig = signatoryConfig ?? throw new ArgumentNullException(nameof(signatoryConfig));
    }

    private void InitCryptoServiceProvider()
    {
        if (_isInitialized)
        {
            return;
        }
        lock (this)
        {
            if (_isInitialized)
            {
                return;
            }
            var pemReader = new PemReader(File.OpenText(_signatoryConfig.Pkcs8PrivateKeyFileAddress));
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pemReader.ReadObject());
            _cryptoServiceProvider = new RSACryptoServiceProvider();
            _cryptoServiceProvider.ImportParameters(rsaParams);
            _isInitialized = true;
        }
    }

    public string Sign(string data)
    {
        InitCryptoServiceProvider();
        var dataBytes = Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(_cryptoServiceProvider.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }

    public string GetKeyId()
    {
        return _signatoryConfig.KeyId;
    }
}