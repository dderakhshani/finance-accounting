using System;
using System.Linq;
using System.Resources;
using Library.Interfaces;

namespace Library.Resources
{
    public interface IResourceFactory
    {
        string GetValidationMessage(string key);
        string GetTranslatedMetaData(string key);
    }

    public interface IMetaDataFactory
    {
        string Get(string key);
    }

    public interface IResourcesFactory
    {
        public ResourceManager ValidationResourceManager { get; }
        public ResourceManager MetaDataResourceManager { get; }
    }


    public class ValidationFactory : IResourceFactory
    {
        private readonly IResourcesFactory _resourcesFactory;
        public ValidationFactory(IResourcesFactory resourcesFactory)
        {
            _resourcesFactory = resourcesFactory;
        }

        public string GetValidationMessage(string key)
        {
            return _resourcesFactory.ValidationResourceManager.GetString(key);
        }

        public string GetTranslatedMetaData(string key)
        {
            return _resourcesFactory.MetaDataResourceManager.GetString(key);
        }
    }

    public class MetaDataFactory : IMetaDataFactory
    {
        private readonly IResourcesFactory _resourcesFactory;
        public MetaDataFactory(IResourcesFactory resourcesFactory)
        {
            _resourcesFactory = resourcesFactory;
        }

        public string Get(string key)
        {
            return _resourcesFactory.MetaDataResourceManager.GetString(key);
        }
    }


    public class ResourcesFactory : IResourcesFactory
    {
        public ResourceManager ValidationResourceManager { get; }
        public ResourceManager MetaDataResourceManager { get; }

        public ResourcesFactory(ICurrentUserAccessor currentUserAccessor)
        {
            //TODO: Load all assemblies from Eefa assesmblies to load microservices custom validation messages
            ValidationResourceManager = new ResourceManager($"Library.Resources.ValidationMsg.Validations_{currentUserAccessor.GetCultureTwoIsoName()}",
                AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName != null && x.FullName.Contains("Library")));

            MetaDataResourceManager = new ResourceManager($"Library.Resources.TranslatedMetaData.{currentUserAccessor.GetCultureTwoIsoName()}",
                AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName != null && x.FullName.Contains("Library")));
        }
    }
}