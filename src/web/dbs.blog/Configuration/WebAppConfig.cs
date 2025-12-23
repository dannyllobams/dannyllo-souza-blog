using dbs.blog.Basics;

namespace dbs.blog.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection ConfigureWebApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.ConfigureAdministrator(configuration);

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.RegisterServices(configuration);

            return services;
        }

        private static void ConfigureAdministrator(this IServiceCollection services, IConfiguration configuration)
        {
            var administratorSettingsSection = configuration.GetSection("AdministratorSettings");
            services.Configure<AdministratorSettings>(administratorSettingsSection);
        }

    }
}
