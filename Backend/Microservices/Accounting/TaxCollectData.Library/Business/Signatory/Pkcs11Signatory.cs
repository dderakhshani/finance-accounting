using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Dto.Config;

namespace TaxCollectData.Library.Business.Signatory;

public class Pkcs11Signatory : ISignatory, ITransferSignatory, IContentSignatory
{
    private readonly Pkcs11SignatoryConfig _pkcs11SignatoryConfig;

    public Pkcs11Signatory(Pkcs11SignatoryConfig pkcs11SignatoryConfig)
    {
        _pkcs11SignatoryConfig = pkcs11SignatoryConfig;
    }

    public string Sign(string data)
    {
        //Initialize PKCS#11
        var factories = new Pkcs11InteropFactories();

        using var pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories,
            _pkcs11SignatoryConfig.LibraryPath,
            AppType.MultiThreaded);
        // Find first slot with token present
        var slot = pkcs11Library.GetSlotList(SlotsType.WithOrWithoutTokenPresent).First();

        // Open RW session
        using var session = slot.OpenSession(SessionType.ReadWrite);
        // Login as normal user
        session.Login(CKU.CKU_USER, _pkcs11SignatoryConfig.UserPin);

        // Specify signing mechanism
        var mechanism = session.Factories.MechanismFactory.Create(CKM.CKM_SHA256_RSA_PKCS);

        var sourceData = ConvertUtils.Utf8StringToBytes(data);

        var pkcs11UriBuilder = new Pkcs11UriBuilder
        {
            ModulePath = _pkcs11SignatoryConfig.LibraryPath,
            PinValue = _pkcs11SignatoryConfig.UserPin,
            Type = CKO.CKO_PRIVATE_KEY
        };

        List<IObjectAttribute> searchTemplate = Pkcs11UriUtils.GetObjectAttributes(new Pkcs11Uri(pkcs11UriBuilder.ToString()),
                session.Factories.ObjectAttributeFactory);

        var allObjects = session.FindAllObjects(searchTemplate);
        var foundObject = allObjects.FirstOrDefault(x =>
                              session.GetAttributeValue(x, new List<CKA>() { CKA.CKA_ID })
                                  .First()
                                  .GetValueAsString()
                                  .StartsWith(_pkcs11SignatoryConfig.KeyId)
                          ) 
                          ?? allObjects.First();

        // Sign data
        var signature = session.Sign(mechanism, foundObject, sourceData);

        session.Logout();
        return ConvertUtils.BytesToBase64String(signature);
    }

    public string GetKeyId()
    {
        return null;
    }
}