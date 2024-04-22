using Microsoft.Extensions.DependencyInjection;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public static class AppServices
    {
        /// <summary>
        /// The service collection for our IoC container
        /// </summary> 
        public static IServiceCollection ServiceCollection { get; private set; } = new ServiceCollection();

        /// <summary>
        /// The service provider for our IoC container
        /// </summary>
        public static IServiceProvider? ServiceProvider { get; private set; }

        public static IServiceManager ServiceManager => Get<IServiceManager>();

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return ServiceProvider.GetService<T>() ?? throw new InvalidOperationException($"Service of type {typeof(T)} not found.");
#pragma warning restore CS8604 // Possible null reference argument.

        }

        private const string DefaultConnectionString = "Data Source=C:\\Users\\nour-\\source\\repos\\MntPlus\\MntPlusDatabase.db";

        /// <summary>
        /// Binds all singleton services
        /// </summary>
        private static void BindServices()
        {
            ServiceCollection.ConfigureRepositoryManager();
            ServiceCollection.ConfigureServiceManager();
            ServiceCollection.ConfigureSqlContext(DefaultConnectionString); 
        }

        public static void Setup()
        {
            // Bind all required services
            BindServices();

            // Build the service provider
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
    }
}
