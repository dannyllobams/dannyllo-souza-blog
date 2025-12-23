namespace dbs.blog.Services
{
    public interface IMediaStorageService
    {
        Task<string> UploadFile(string fileName, Stream content, CancellationToken cancellationToken = default);
    }
}
