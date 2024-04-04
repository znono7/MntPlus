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


    }
}
