using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IRedisDataProvider
    {
        //void Set<T>(string key, T value);
        
        Task<bool> FlushServer();

        void Set<T>(string key, T value, TimeSpan timeout);

        T Get<T>(string key);

        IList<T> GetAll<T>();


        bool Remove(string key);

        bool IsInCache(string key);

        public void Update<T>(string key, T value, TimeSpan timeout);

        //public void Update<T>(string key, T value);
    }
}