using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models;
using MongoDB.Driver;
using Repository;
using Repository.Interfaces;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure DI for application services
            var databaseName = Configuration.GetSection("ConnectionStrings")["DatabaseName"];
            var connectionString = Configuration.GetSection("ConnectionStrings")["ConnectionString"];

            services.AddScoped<IMongoClient, MongoClient>(provider => new MongoClient(connectionString));
            services.AddScoped(provider =>
            {
                var service = (IMongoClient)provider.GetService(typeof(IMongoClient));
                return service.GetDatabase(databaseName);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService<User>, AuthService<User>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMvc();
        }
    }
}
