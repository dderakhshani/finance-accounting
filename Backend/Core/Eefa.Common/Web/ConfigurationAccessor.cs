using Microsoft.Extensions.Configuration;

namespace Eefa.Common.Web
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        private readonly IConfiguration _configuration;

        public ConfigurationAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IJwtConfigurationModel GetJwtConfiguration()
        {
            return _configuration.GetSection("Jwt").Get<JwtConfigurationModel>();
        }

        public IRedisConfigurationModel GetRedisConfiguration()
        {
            return _configuration.GetSection("Redis").Get<RedisConfigurationModel>();
        }

        public ISwaggerConfigurationModel GetSwaggerConfiguration()
        {
            return _configuration.GetSection("Swagger").Get<SwaggerConfigurationModel>();
        }

        public ICorsConfigurationModel GetCorsConfiguration()
        {
            return _configuration.GetSection("Cors").Get<CorsConfigurationModel>();
            
        }

        //public IAuditMongoDbConfigurationModel GetAuditMongoDbConfiguration()
        //{
        //    return _configuration.GetSection("AuditMongoDb").Get<AuditMongoDbConfigurationModel>();
        //}

        public IConnectionStringModel GetConnectionString()
        {
            return new ConnectionStringModel
            {
                DefaultString = _configuration.GetConnectionString("DefaultString")
            };
        }
        public IConnectionStringModel GetSamaConnectionString()
        {
            return new ConnectionStringModel
            {
                DefaultString = _configuration.GetConnectionString("SamaString")
            };
        }
        public IoPath GetIoPaths()
        {
            return _configuration.GetSection("IoPath").Get<IoPath>();
        }
        public AssymetricKeysModel GetAssymetricKeys()
        {
            return _configuration.GetSection("AsymetricKeyPair").Get<AssymetricKeysModel>();
        }

    }
}