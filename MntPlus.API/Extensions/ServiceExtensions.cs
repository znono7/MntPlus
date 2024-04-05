using Contracts;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;

namespace MntPlus.API
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// CORS (Cross-Origin Resource Sharing) is a security feature that restricts cross-origin HTTP requests that are initiated from scripts running in the browser.
        /// is a mechanism to give or restrict access rights to applications from different domains.
        /// </summary>
        /// <param name="services">  </param>
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                                   builder.AllowAnyOrigin()
                                                          .AllowAnyMethod()
                                                                                 .AllowAnyHeader());
            });

        /// <summary>
        /// IIS (Internet Information Services) is a flexible, secure, and manageable Web server for hosting anything on the Web.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {
            });

        /// <summary>
        ///   register the Repository manager class in the service container
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
                                services.AddScoped<IRepositoryManager, RepositoryManager>();

        /// <summary>
        ///  register the Service manager class in the service container
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServiceManager(this IServiceCollection services) =>
                                services.AddScoped<IServiceManager, ServiceManager>();

        /// <summary>
        /// register the SqlContext class in the service container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSqlContext(this IServiceCollection services,
                        IConfiguration configuration) =>
                            services.AddDbContext<RepositoryContext>(opts =>
                                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));


    }
}
