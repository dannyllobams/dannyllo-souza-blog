using Azure.Storage.Blobs;
using Cortex.Mediator.DependencyInjection;
using dbs.blog.Basics;
using dbs.blog.Services;
using dbs.core.Mediator;
using dbs.domain.Repositories;
using dbs.infra.Context;
using dbs.infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace dbs.blog.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediaStorageService(configuration);
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IPostsRepository, PostsRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPageViewRepository, PageViewRepository>();

            services.AddCortexMediator(
                configuration: configuration,
                handlerAssemblyMarkerTypes: new[] { typeof(Program) },
                configure: options =>
                {
                    options.AddDefaultBehaviors();
                }
            );
        }

        private static void AddMediaStorageService(this IServiceCollection services, IConfiguration configuration)
        {
            var blobServiceSettingsSection = configuration.GetSection("BlobServiceSettings");
            services.Configure<BlobServiceSettings>(blobServiceSettingsSection);

            var blobServiceSettings = blobServiceSettingsSection.Get<BlobServiceSettings>();

            services.AddSingleton(_ =>
            {
                var blobService = new BlobServiceClient(blobServiceSettings!.ConnectionString);
                var containerClient = blobService.GetBlobContainerClient(blobServiceSettings.ContainerName);

                containerClient.CreateIfNotExists();

                return blobService;
            });

            services.AddScoped<IMediaStorageService, MediaStorageService>();
        }
    }
}
