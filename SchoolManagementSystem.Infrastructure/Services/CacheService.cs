using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SchoolManagementSystem.Application.Interfaces;

namespace SchoolManagementSystem.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        private readonly JsonSerializerSettings _defaultSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public string SetSecrets(string key, string value)
        {
            _cache.Set(key, value);
            return value;
        }

        public void RemoveSecrets(string key)
        {
            _cache.Remove(key);
        }

        public string GetSecrets(string key)
        {
            return _cache.TryGetValue(key, out string value)
                ? value!
                : string.Empty;
        }
        public T GetData<T>(string key)
        {
            if (_cache.TryGetValue(key, out string json))
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    return JsonConvert.DeserializeObject<T>(json, _defaultSettings);
                }
            }
            return default!;
        }
        public bool SetData<T>(string key, T value)
        {
            string json = JsonConvert.SerializeObject(value, _defaultSettings);

            _cache.Set(key, json);

            return true;
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}