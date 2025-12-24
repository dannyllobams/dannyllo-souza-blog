using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace dbs.blog.Services
{
    public sealed class MemoryCacheService : IMemoryCacheService
    {
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(10);

        private readonly IMemoryCache _cache;
        private readonly byte[] _saltBytes;

        public MemoryCacheService(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _saltBytes = Guid.NewGuid().ToByteArray();
        }


        public bool TryGet(string key, out object? value) => _cache.TryGetValue(key, out value);

        public bool TryGet<T>(string key, out T? value) => _cache.TryGetValue(key, out value);

        public void Set<T>(string key, T value, TimeSpan? ttl = null)
        {
            _cache.Set(
                key,
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl ?? DefaultTtl
                }
            );
        }

        public void Remove(string key) => _cache.Remove(key);

        public bool TryGetHashed(string key, out object? value) =>
            _cache.TryGetValue(ComputeHmacHex(key), out value);

        public bool TryGetHashed<T>(string key, out T? value) =>
            _cache.TryGetValue(ComputeHmacHex(key), out value);

        public void SetHashed<T>(string key, T value, TimeSpan? ttl = null)
        {
            _cache.Set(
                ComputeHmacHex(key),
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl ?? DefaultTtl
                }
            );
        }

        public void RemoveHashed(string key) => _cache.Remove(ComputeHmacHex(key));

        public long BumpVersion(string namespaceKey)
        {
            var versionKey = VersionKey(namespaceKey);

            var current = _cache.TryGetValue<long>(versionKey, out var v) ? v : 1;
            var next = current + 1;

            _cache.Set(versionKey, next, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365)
            });

            return next;
        }

        public long GetVersion(string namespaceKey, long defaultVersion = 1)
        {
            var versionKey = VersionKey(namespaceKey);

            if (_cache.TryGetValue<long>(versionKey, out var v))
                return v;

            _cache.Set(versionKey, defaultVersion, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365)
            });

            return defaultVersion;
        }

        public string BuildVersionedKey(string namespaceKey, string key)
        {
            var v = GetVersion(namespaceKey);
            return $"{namespaceKey}:v{v}:{key}";
        }

        public string BuildVersionedHashedKey(string namespaceKey, string key)
        {
            var v = GetVersion(namespaceKey);
            return $"{namespaceKey}:v{v}:{ComputeHmacHex(key)}";
        }

        private static string VersionKey(string ns) => $"__ver__:{ns}";

        private string ComputeHmacHex(string value)
        {
            using var hmac = new HMACSHA256(_saltBytes);
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = hmac.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}