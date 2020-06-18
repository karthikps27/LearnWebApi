using LearnWebApi.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace LearnWebApi.Infrastructure
{
    public interface ILocalCache
    {
        UserData Get(string key);
        void Set(string key, UserData userdata);
        bool KeyExists(string key);
    }

    public class LocalCache : ILocalCache
    {        
        private Dictionary<string, string> _passwords;
        private readonly IMemoryCache _memoryCache;

        private TimeSpan LocalExpiry => TimeSpan.FromMinutes(5);

        public LocalCache()
        {
            _passwords = new Dictionary<string, string>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public UserData Get(string key)
        {
            _memoryCache.TryGetValue(key, out UserData userdata);
            
            return userdata;
        }

        public bool KeyExists(string key)
        {
            return _memoryCache.TryGetValue(key, out object _);
        }

        public void Set(string key, UserData value)
        {
            _memoryCache.Set(key, value, LocalExpiry);
        }
    }
}
