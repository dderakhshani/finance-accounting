using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Configurations
{
    public class ValuesProvider : IValuesProvider
    {
        private static readonly Random Random = new Random();
        private static readonly IDictionary<int, string> Data = new ConcurrentDictionary<int, string>();

        public async Task<string> GetAsync(int id)
        {
            Data.TryGetValue(id, out string value);
            return await Task.FromResult(value);
        }

        public IEnumerable<string> GetValues()
        {
            return Data.Values;
        }

        public async Task<int> InsertAsync(string value)
        {
            int key = Random.Next();
            Data[key] = value;
            return await Task.FromResult(key);
        }

        public async Task ReplaceAsync(int id, string value)
        {
            Data[id] = value;
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult(Data.Remove(id));
        }

        public async Task<int> DeleteMultipleAsync(int[] ids)
        {
            int c = 0;
            foreach (int id in ids)
            {
                c += await DeleteAsync(id) ? 1 : 0;
            }
            return c;
        }

    }
}