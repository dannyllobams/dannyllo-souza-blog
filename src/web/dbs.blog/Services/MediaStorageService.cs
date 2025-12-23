using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using dbs.blog.Basics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace dbs.blog.Services
{
    public class MediaStorageService : IMediaStorageService
    {
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
        private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

        private readonly BlobContainerClient _containerClient;

        public MediaStorageService(BlobServiceClient blobServiceClient, IOptions<BlobServiceSettings> blobServiceSettings)
        {
            var _blobServiceSettings  = blobServiceSettings.Value;

            _containerClient = blobServiceClient.GetBlobContainerClient(_blobServiceSettings.ContainerName);
        }

        public async Task<string> UploadFile(string fileName, Stream content, CancellationToken cancellationToken = default)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("File Type not allowed.");
            }

            if (content == null || content.Length == 0)
                throw new ArgumentException("Empty file.");

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var blobClient = _containerClient.GetBlobClient(uniqueFileName);

            if (!ContentTypeProvider.TryGetContentType(uniqueFileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            await blobClient.UploadAsync(content,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType,
                        CacheControl = "public, max-age=31536000, immutable"
                    }
                }, cancellationToken);

            return uniqueFileName;
        }
    }
}
