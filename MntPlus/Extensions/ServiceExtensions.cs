using Contracts;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service.Contracts;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MntPlus.WPF
{
    public static class ServiceExtensions
    {
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
                        string connection) =>
                            services.AddDbContext<RepositoryContext>(opts =>
                                    opts.UseSqlite(connection));
    }
}
