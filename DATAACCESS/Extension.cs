
using SECRYPT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DATAACCESS
{
    public static class Extension
    {
        private static readonly string dataBaseSectionName = "database";
        private static readonly string segurinetSectionName = "segurinet";
        public static IServiceCollection AddDataBase(this IServiceCollection services)
        {
            IConfiguration? configuration;
            var serviceProvider = services.BuildServiceProvider();
            configuration = serviceProvider.GetService<IConfiguration>();
            DBOptions optionsDB = new DBOptions();
            configuration!.GetSection(dataBaseSectionName).Bind(optionsDB);
            SecryptOptions optionsSecrypt = new SecryptOptions();
            configuration.GetSection(segurinetSectionName).Bind(optionsSecrypt);
            services.AddSingleton<IDataAccess, DataAccess>(sp => { return new DataAccess(optionsDB, optionsSecrypt); });
            return services;
        }
    }
}
