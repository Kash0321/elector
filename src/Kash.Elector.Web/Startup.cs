using Kash.CrossCutting.Cache;
using Kash.CrossCutting.Cache.InMemory;
using Kash.CrossCutting.Cache.Redis;
using Kash.Elector.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kash.Elector.Web
{
    public class Startup
    {
        //const string SECRET_NAME = "CacheConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // InMemory
            //services.AddMemoryCache();

            // Redis
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = Configuration[SECRET_NAME];
            //    options.InstanceName = "RedisInstance";
            //});

            services.AddScoped<IDistrictRepository, DummyDistrictRepository>();
            //services.AddSingleton<IDistrictRepository, DummyDistrictRepositoryWithInnerStaticCache>();
            //services.AddScoped<IDistrictRepository, DummyDistrictRepositoryWithCache>();
            //services.AddSingleton<ICacheManager, InMemoryCacheManager>();
            //services.AddScoped<ICacheManager, RedisCacheManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var connection = @"Server=(localdb)\mssqllocaldb;Database=electorDB;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ElectorContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
