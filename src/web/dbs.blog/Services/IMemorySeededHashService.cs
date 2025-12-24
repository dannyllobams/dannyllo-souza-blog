namespace dbs.blog.Services
{
    public interface IMemorySeededHashService
    {
        string ComputeHash(string value);
    }
}
