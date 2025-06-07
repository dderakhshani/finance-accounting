namespace Eefa.NotificationServices.Authorization
{
    public class JwtConfigurations
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
    }
}
