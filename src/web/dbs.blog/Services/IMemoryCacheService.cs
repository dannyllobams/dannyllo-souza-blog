namespace dbs.blog.Services
{
    public interface IMemoryCacheService
    {
        bool TryGet(string key, out object? value);
        bool TryGet<T>(string key, out T? value);
        void Set<T>(string key, T value, TimeSpan? ttl = null);
        void Remove(string key);

        bool TryGetHashed(string key, out object? value);
        bool TryGetHashed<T>(string key, out T? value);
        void SetHashed<T>(string key, T value, TimeSpan? ttl = null);
        void RemoveHashed(string key);

        long BumpVersion(string namespaceKey);
        long GetVersion(string namespaceKey, long defaultVersion = 1);

        string BuildVersionedKey(string namespaceKey, string key);
        string BuildVersionedHashedKey(string namespaceKey, string key);
    }
}
