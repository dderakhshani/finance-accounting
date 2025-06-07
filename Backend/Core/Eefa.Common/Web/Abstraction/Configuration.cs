namespace Eefa.Common.Web
{
    public interface IConfigurationAccessor
    {
        public IConnectionStringModel GetConnectionString();
        public IJwtConfigurationModel GetJwtConfiguration();
        public IRedisConfigurationModel GetRedisConfiguration();
        public ISwaggerConfigurationModel GetSwaggerConfiguration();
        public ICorsConfigurationModel GetCorsConfiguration();
        //public IAuditMongoDbConfigurationModel GetAuditMongoDbConfiguration();
        public IoPath GetIoPaths();
        public AssymetricKeysModel GetAssymetricKeys();
    }

    public interface IConnectionStringModel
    {
        public string DefaultString { get; set; }
    }

    public interface ICorsConfigurationModel
    {
        public string PolicyName { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public bool AllowAnyMethod { get; set; }
        public bool AllowAnyHeader { get; set; }
    }

    public interface IJwtConfigurationModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpirySecondTime { get; set; }
        public int RefreshTokenExpirySecondTime { get; set; }
    }

    public interface IRedisConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }

    public interface IRedisConfigurationModel
    {
        public string Redis { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int ExpirySecondtime { get; set; }
    }

    public interface ISwaggerConfigurationModel
    {
        public string XmlPath { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }


    //public interface IAuditMongoDbConfigurationModel
    //{
    //    public string ConnectionString { get; set; }
    //    public string Database { get; set; }
    //    public string Collection { get; set; }
    //    public bool UseBson { get; set; }
    //}


}
