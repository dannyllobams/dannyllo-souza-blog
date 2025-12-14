namespace dbs.blog.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection ConfigureWebApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.RegisterServices(configuration);

            return services;
        }
    }
}
