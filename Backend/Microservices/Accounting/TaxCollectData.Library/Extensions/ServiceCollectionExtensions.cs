using Microsoft.Extensions.DependencyInjection;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Abstraction.Signatory;
using TaxCollectData.Library.Api;
using TaxCollectData.Library.Business;
using TaxCollectData.Library.Business.Normalize;
using TaxCollectData.Library.Business.Signatory;
using TaxCollectData.Library.Dto.Config;

namespace TaxCollectData.Library.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaxApi<TTransferSignatory>(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        EncryptionConfig encryptionConfig = null) where TTransferSignatory : class, ISignatory, ITransferSignatory, IContentSignatory
    {
        serviceDescriptors.AddServiceCollection(baseUrl, clientId, properties, encryptionConfig);
        serviceDescriptors.AddSingleton<ISignatory, TTransferSignatory>();
        serviceDescriptors.AddSingleton<ITransferSignatory, TTransferSignatory>();
        serviceDescriptors.AddSingleton<IContentSignatory, TTransferSignatory>();
        return serviceDescriptors;
    }
    public static IServiceCollection AddTaxApi<TTransferSignatory, TContentSignatory>(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        EncryptionConfig encryptionConfig = null)
        where TTransferSignatory : class, ITransferSignatory 
        where TContentSignatory : class, IContentSignatory
    {
        serviceDescriptors.AddServiceCollection(baseUrl, clientId, properties, encryptionConfig);
        serviceDescriptors.AddSingleton<ITransferSignatory, TTransferSignatory>();
        serviceDescriptors.AddSingleton<IContentSignatory, TContentSignatory>();
        return serviceDescriptors;
    }
    
    public static IServiceCollection AddTaxApi(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        SignatoryConfig transferSignatoryConfig,
        SignatoryConfig? contentSignatoryConfig = null,
        EncryptionConfig encryptionConfig = null)
    {
        serviceDescriptors.AddServiceCollection(baseUrl, clientId, properties, encryptionConfig);
        serviceDescriptors.AddSingleton<ITransferSignatory, InMemorySignatory>(_ => new InMemorySignatory(transferSignatoryConfig));
        serviceDescriptors.AddSingleton<IContentSignatory, InMemorySignatory>(_ => new InMemorySignatory(contentSignatoryConfig ?? transferSignatoryConfig));
        return serviceDescriptors;
    }
    
    public static IServiceCollection AddTaxApi(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        Pkcs8SignatoryConfig transferSignatoryConfig,
        Pkcs8SignatoryConfig? contentSignatoryConfig = null,
        EncryptionConfig encryptionConfig = null)
    {
        serviceDescriptors.AddServiceCollection(baseUrl, clientId, properties, encryptionConfig);
        serviceDescriptors.AddSingleton<ITransferSignatory, Pkcs8Signatory>(_ => new Pkcs8Signatory(transferSignatoryConfig));
        serviceDescriptors.AddSingleton<IContentSignatory, Pkcs8Signatory>(_ => new Pkcs8Signatory(contentSignatoryConfig ?? transferSignatoryConfig));
        return serviceDescriptors;
    }
    
    public static IServiceCollection AddTaxApi(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        Pkcs11SignatoryConfig transferSignatoryConfig,
        Pkcs11SignatoryConfig? contentSignatoryConfig = null,
        EncryptionConfig encryptionConfig = null)
    {
        serviceDescriptors.AddServiceCollection(baseUrl, clientId, properties, encryptionConfig);
        serviceDescriptors.AddSingleton<ITransferSignatory, Pkcs11Signatory>(_ => new Pkcs11Signatory(transferSignatoryConfig));
        serviceDescriptors.AddSingleton<IContentSignatory, Pkcs11Signatory>(_ => new Pkcs11Signatory(contentSignatoryConfig ?? transferSignatoryConfig));
        return serviceDescriptors;
    }

    private static IServiceCollection AddServiceCollection(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string clientId,
        IProperties properties,
        EncryptionConfig encryptionConfig)
    {
        serviceDescriptors.AddSingleton<IPacketDtoAdapter, PacketDtoAdapter>();
        serviceDescriptors.AddSingleton<IEncryptor, DefaultEncryptor>();
        serviceDescriptors.AddSingleton<IVerhoffProvider, VerhoffProvider>();
        serviceDescriptors.AddSingleton<ITaxApis, DefaultTaxApiClient>(p =>
            new DefaultTaxApiClient(p.GetService<ITransferApi>() ?? throw new ArgumentNullException(),
                clientId,
                p.GetService<EncryptionConfig>() ?? throw new ArgumentNullException(),
                properties));
        serviceDescriptors.AddSingleton(encryptionConfig ?? new EncryptionConfig());
        serviceDescriptors.AddSingleton<IPacketCodec, PacketCodec>();
        serviceDescriptors.AddSingleton<ITaxIdGenerator, TaxIdGenerator>();
        serviceDescriptors.AddApiVersionBasedConfigs(baseUrl, properties.ApiVersion, properties.TokenHeaderName, properties.CustomHeaders);
        return serviceDescriptors;
    }

    private static IServiceCollection AddApiVersionBasedConfigs(this IServiceCollection serviceDescriptors,
        string baseUrl,
        string apiVersion,
        string tokenHeaderName,
        Dictionary<string, string> customHeaders)
    {
        baseUrl = baseUrl.EndsWith("/") ? baseUrl : $"{baseUrl}/";
        serviceDescriptors.AddHttpClientByUri(new Uri(baseUrl));
        if (string.IsNullOrWhiteSpace(apiVersion))
        {
            serviceDescriptors.AddSingleton<INormalizer, ObjectNormalizer>(_ => new(customHeaders, tokenHeaderName));
            serviceDescriptors.AddSingleton<ITransferApi, ObjectTransferApi>();
        }
        else if (apiVersion.Equals("v1", StringComparison.InvariantCultureIgnoreCase))
        {
            serviceDescriptors.AddSingleton<INormalizer, SimpleNormalizer>(_ => new(tokenHeaderName));
            serviceDescriptors.AddSingleton<ITransferApi, SimpleTransferApi>();
        }

        return serviceDescriptors;
    }

    private static IServiceCollection AddHttpClientByUri(this IServiceCollection serviceDescriptors, Uri uri)
    {
        serviceDescriptors.AddHttpClient<IHttpRequestSender, RestSharpHttpRequestSender>(client => client.BaseAddress = uri)
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            });
        return serviceDescriptors;
    }
}