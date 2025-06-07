using Org.BouncyCastle.OpenSsl;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Dto.Config;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;


namespace TaxCollectData.Library.Business.Signatory;

internal class InMemorySignatory : ISignatory, ITransferSignatory, IContentSignatory 
{
    private readonly SignatoryConfig _signatoryConfig;
    private RSACryptoServiceProvider _cryptoServiceProvider;
    private bool _isInitialized;

    public InMemorySignatory(SignatoryConfig signatoryConfig)
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
            var pem = $"-----BEGIN PRIVATE KEY-----\n{_signatoryConfig.PrivateKey}\n-----END PRIVATE KEY-----"; // Add header and footer
            var pemReader = new PemReader(new StringReader(pem));
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pemReader.ReadObject());
            _cryptoServiceProvider = new RSACryptoServiceProvider();
            _cryptoServiceProvider.ImportParameters(rsaParams);
            _isInitialized = true;
        }
    }

    public string Sign(string stringToBeSigned)
    {
        InitCryptoServiceProvider();
        var dataBytes = Encoding.UTF8.GetBytes(stringToBeSigned);
        return Convert.ToBase64String(_cryptoServiceProvider.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }

    public string GetKeyId()
    {
        return _signatoryConfig.KeyId;
    }
}