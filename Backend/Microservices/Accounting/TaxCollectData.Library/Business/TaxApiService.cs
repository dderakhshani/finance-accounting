using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Dto.Config;
using TaxCollectData.Library.Exceptions;
using TaxCollectData.Library.Extensions;

namespace TaxCollectData.Library.Business;

public class TaxApiService
{
    private const string BASE_URL = "http://master.nta.local/requestsmanager/api/";

    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private IServiceProvider _serviceProvider;
    
    public static TaxApiService Instance { get; set; } = new();
    private TaxApiService()
    {
    }
    
    private ITaxApis _taxApis;
    public ITaxApis TaxApis
    {
        get => _serviceProvider.GetService<ITaxApis>() ?? throw new NotInitializedException(nameof(_taxApis));
        private set => _taxApis = value;
    }

    private ITransferApi _transferApi;
    public ITransferApi TransferApi
    {
        get => _serviceProvider.GetService<ITransferApi>() ?? throw new NotInitializedException(nameof(_transferApi));
        private set => _transferApi = value;
    }
    
    private ITaxIdGenerator _taxIdGenerator;
    public ITaxIdGenerator TaxIdGenerator
    {
        get => _serviceProvider.GetService<ITaxIdGenerator>() ?? throw new NotInitializedException(nameof(_taxIdGenerator));
        private set => _taxIdGenerator = value;
    }

    public void Init(string clientId,
        SignatoryConfig transferSignatoryConfig,
        IProperties properties,
        string baseUrl = BASE_URL,
        EncryptionConfig encryptionConfig = null,
        SignatoryConfig? contentSignatoryConfig = null)
    {
        _serviceCollection.Clear();
        _serviceCollection.AddTaxApi(baseUrl,
            clientId,
            properties,
            transferSignatoryConfig,
            contentSignatoryConfig,
            encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
    
    public void Init(string clientId,
        Pkcs8SignatoryConfig transferSignatoryConfig,
        IProperties properties,
        string baseUrl = BASE_URL,
        EncryptionConfig encryptionConfig = null,
        Pkcs8SignatoryConfig? contentSignatoryConfig = null)
    {
        _serviceCollection.Clear();
        _serviceCollection.AddTaxApi(baseUrl,
            clientId,
            properties,
            transferSignatoryConfig,
            contentSignatoryConfig,
            encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
    
    public void Init(string clientId,
        Pkcs11SignatoryConfig transferSignatoryConfig,
        IProperties properties,
        string baseUrl = BASE_URL,
        EncryptionConfig encryptionConfig = null,
        Pkcs11SignatoryConfig? contentSignatoryConfig = null)
    {
        _serviceCollection.Clear();
        _serviceCollection.AddTaxApi(baseUrl,
            clientId,
            properties,
            transferSignatoryConfig,
            contentSignatoryConfig,
            encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
    
    public void Init<TTransferSignatory>(string clientId,
        IProperties properties,
        string baseUrl = BASE_URL,
        EncryptionConfig encryptionConfig = null) where TTransferSignatory : class, ISignatory, ITransferSignatory, IContentSignatory
    {
        _serviceCollection.Clear();
        _serviceCollection.AddTaxApi<TTransferSignatory>(baseUrl,
            clientId,
            properties,
            encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
    public void Init<TTransferSignatory, TContentSignatory>(string clientId,
        IProperties properties,
        string baseUrl = BASE_URL,
        EncryptionConfig encryptionConfig = null)
        where TTransferSignatory : class, ITransferSignatory 
        where TContentSignatory : class, IContentSignatory
    {
        _serviceCollection.Clear();
        _serviceCollection.AddTaxApi<TTransferSignatory, TContentSignatory>(baseUrl,
            clientId,
            properties,
            encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
    }
}