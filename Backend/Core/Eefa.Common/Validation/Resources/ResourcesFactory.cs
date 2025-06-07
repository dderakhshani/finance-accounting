using System.Resources;

namespace Eefa.Common.Validation.Resources
{
    public interface IValidationFactory
    {
        string Get(string key);
    }

    public interface IResourceFactory
    {
        public ResourceManager ResourceManager { get; }
    }

    public class ValidationFactory : IValidationFactory
    {
        private readonly IResourceFactory _resourcesFactory;
        public ValidationFactory(IResourceFactory resourcesFactory)
        {
            _resourcesFactory = resourcesFactory;
        }

        public string Get(string key)
        {
            return _resourcesFactory.ResourceManager.GetString(key);
        }
    }


    public class ResourcesFactory : IResourceFactory
    {
        public ResourceManager ResourceManager { get; }
        public ResourcesFactory(ICurrentUserAccessor currentUserAccessor)
        {
            //Eefa.Common.Validation.Resources.ValidationMsg
            ResourceManager = new ResourceManager(
                $"{typeof(Eefa.Common.Validation.Resources.Validations_en).Namespace}.Validations_{currentUserAccessor.GetCultureTwoIsoName()}",
                typeof(ResourcesFactory).Assembly);
        }
    }
}