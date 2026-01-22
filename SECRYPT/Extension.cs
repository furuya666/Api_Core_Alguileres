using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SECRYPT
{
    public static class Extension
    {
        private static readonly string segurinetSectionName = "segurinet";
        public static IServiceCollection AddSecrypt(this IServiceCollection services)
        {

            IConfiguration? configuration;
            var serviceProvider = services.BuildServiceProvider();
            configuration = serviceProvider.GetService<IConfiguration>();
            SecryptOptions options = new SecryptOptions();
            configuration?.GetSection(segurinetSectionName).Bind(options);
            services.AddSingleton<IManagerSecrypt, ManagerSecrypt>(sp => { return new ManagerSecrypt(options.Semilla); });
            return services;
        }
    }
}
