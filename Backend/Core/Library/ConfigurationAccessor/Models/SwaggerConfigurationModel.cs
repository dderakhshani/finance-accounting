using Library.Interfaces;

namespace Library.ConfigurationAccessor.Models
{
    public class SwaggerConfigurationModel : ISwaggerConfigurationModel
    {
        public string XmlPath { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}