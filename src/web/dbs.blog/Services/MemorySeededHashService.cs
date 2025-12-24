using System.Security.Cryptography;
using System.Text;

namespace dbs.blog.Services
{
    public class MemorySeededHashService : IMemorySeededHashService
    {
        private readonly byte[] _hashSalt;

        public MemorySeededHashService()
        {
            _hashSalt = Guid.NewGuid().ToByteArray();
        }

        public string ComputeHash(string value)
        {
            using var sha = SHA256.Create();

            var valueBytes = Encoding.UTF8.GetBytes(value);
            var bytes = new byte[_hashSalt.Length + valueBytes.Length];

            Buffer.BlockCopy(_hashSalt, 0, bytes, 0, _hashSalt.Length);
            Buffer.BlockCopy(valueBytes, 0, bytes, _hashSalt.Length, valueBytes.Length);

            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
