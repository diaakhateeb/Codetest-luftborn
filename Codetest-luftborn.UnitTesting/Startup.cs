using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Repository;
using Repository.Interfaces;
using System.IO;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace Tests
{
    public class Startup
    {
        public static void Initialize()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + @"..\..\..\..\")
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var databaseName = configuration.GetSection("ConnectionStrings")["DatabaseName"];
            var connectionString = configuration.GetSection("ConnectionStrings")["ConnectionString"];

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddScoped<IMongoClient, MongoClient>(provider => new MongoClient(connectionString));
            serviceCollection.AddScoped(provider =>
            {
                var service = (IMongoClient)provider.GetService(typeof(IMongoClient));
                return service.GetDatabase(databaseName);
            });
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserService, UserService>();
        }
    }
}
