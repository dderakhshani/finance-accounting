using Library.ConfigurationAccessor.Models;

namespace Library.Interfaces
{
    public interface IConfigurationAccessor
    {
        public IConnectionStringModel GetConnectionString(string title = "DefaultString");
        public IJwtConfigurationModel GetJwtConfiguration();
        public IRedisConfigurationModel GetRedisConfiguration();
        public ISwaggerConfigurationModel GetSwaggerConfiguration();
        public ICorsConfigurationModel GetCorsConfiguration();
        //public IAuditMongoDbConfigurationModel GetAuditMongoDbConfiguration();
        public IoPath GetIoPaths();
        public AssymetricKeysModel GetAssymetricKeys();
        public bool GetUseInMemoryDatabase();
    }
}