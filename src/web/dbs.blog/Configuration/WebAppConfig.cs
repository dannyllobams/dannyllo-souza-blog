namespace dbs.blog.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection ConfigureWebApp(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
