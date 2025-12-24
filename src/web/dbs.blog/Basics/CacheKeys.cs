namespace dbs.blog.Basics
{
    public static class CacheKeys
    {
        public const string POSTS_NAMESPACE = "ns_posts";
        public const string BLOG_POSTS_COUNT = "blog:posts:count";

        public static string BLOG_POSTS(bool published, int page, int pageSize) => $"blog:posts:pub{(published ? 1 : 0)}:pn{page}:ps{pageSize}";
        public static string BLOG_POST(string url) => $"blog:{url}";
    }
}
