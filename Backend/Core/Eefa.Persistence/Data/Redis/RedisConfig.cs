using Library.Interfaces;

namespace Eefa.Persistence.Data.Redis
{
    public class RedisConfig : IRedisConfig
    {
        private string _host;
        private int _port;
        private string _password;


        public RedisConfig(string host, int port, string password)
        {
            this._host = host;
            this._port = port;
            this._password = password;
        }

        string IRedisConfig.Host
        {
            get => _host;
            set => _host = value;
        }

        int IRedisConfig.Port
        {
            get => _port;
            set => _port = value;
        }

        string IRedisConfig.Password
        {
            get => _password;
            set => _password = value;
        }
    }
}