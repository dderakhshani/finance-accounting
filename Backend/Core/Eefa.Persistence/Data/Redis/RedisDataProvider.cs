using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Interfaces;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace Eefa.Persistence.Data.Redis
{
    public class RedisDataProvider : IRedisDataProvider
    {
        private readonly RedisEndpoint _endPoint;
        private readonly IRedisConfig _redisConfig;
        public RedisDataProvider(IRedisConfig redisConfig)
        {
            this._redisConfig = redisConfig;
            _endPoint = new RedisEndpoint(redisConfig.Host, redisConfig.Port, redisConfig.Password);
        }


        //public void Set<T>(string key, T value)
        //{
        //    this.Set(key, value, TimeSpan.Zero);
        //}

        public async Task<bool> FlushServer()
        {
            using (var redis = await ConnectionMultiplexer.ConnectAsync(
                $"{_redisConfig.Host}:{_redisConfig.Port},password={_redisConfig.Password},allowAdmin=true"))
            {
                foreach (var endPoint in redis.GetEndPoints())
                {
                    var server = redis.GetServer(endPoint);
                    await server.FlushAllDatabasesAsync();
                }
            }
            return true;
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            using var client = new RedisClient(_endPoint);
            client.As<T>().SetValue(key, value, timeout);
        }

        public T Get<T>(string key)
        {
            using var client = new RedisClient(_endPoint);
            var wrapper = client.As<T>();
            return wrapper.GetValue(key);
        }

        public IList<T> GetAll<T>()
        {
            using var client = new RedisClient(_endPoint);
            return client.GetAll<T>();
        }

        public bool Remove(string key)
        {
            using var client = new RedisClient(_endPoint);
            return client.Remove(key);
        }

        public bool IsInCache(string key)
        {
            using var client = new RedisClient(_endPoint);
            return client.ContainsKey(key);
        }

        public void Update<T>(string key, T value, TimeSpan timeout)
        {
            Remove(key);
            Set(key, value, timeout);
        }

        //public void Update<T>(string key, T value)
        //{
        //    Remove(key);
        //    Set(key, value);
        //}
    }
}