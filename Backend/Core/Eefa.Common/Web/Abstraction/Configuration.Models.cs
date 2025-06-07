namespace Eefa.Common.Web
{
    public class AssymetricKeysModel
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }

    //public class AuditMongoDbConfigurationModel : IAuditMongoDbConfigurationModel
    //{
    //    public string ConnectionString { get; set; }
    //    public string Database { get; set; }
    //    public string Collection { get; set; }
    //    public bool UseBson { get; set; }
    //}

    public class ConnectionStringModel : IConnectionStringModel
    {
        public string DefaultString { get; set; }
    }

    public class CorsConfigurationModel : ICorsConfigurationModel
    {
        public string PolicyName { get; set; }
        public bool AllowAnyOrigin { get; set; }
        public bool AllowAnyMethod { get; set; }
        public bool AllowAnyHeader { get; set; }

    }
    public class IoPath
    {
        public string Root { get; set; }
        public string PersonProfile { get; set; }
        public string PersonSignature { get; set; }
        public string Temp { get; set; }
        public string Attachment { get; set; }
        public string FlagImage { get; set; }

    }

    public class JwtConfigurationModel : IJwtConfigurationModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpirySecondTime { get; set; }
        public int RefreshTokenExpirySecondTime { get; set; }
    }

    public class RedisConfigurationModel : IRedisConfigurationModel
    {
        public string Redis { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int ExpirySecondtime { get; set; }
    }

    public class SwaggerConfigurationModel : ISwaggerConfigurationModel
    {
        public string XmlPath { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

}
