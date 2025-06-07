using Library.Interfaces;

namespace Library.ConfigurationAccessor.Models
{
    public class CorsConfigurationModel: ICorsConfigurationModel
    {
        public string PolicyName { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public bool AllowAnyMethod { get; set; }
        public bool AllowAnyHeader { get; set; }
    }
}