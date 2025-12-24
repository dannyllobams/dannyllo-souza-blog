using Microsoft.AspNetCore.Authentication.Cookies;

namespace dbs.blog.Configuration
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "dbs.blog.admin.auth";
                options.SlidingExpiration = true;

                options.LoginPath = "/Admin/auth/login";
            });

            return services;
        }
    }
}
