using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using LearnWebApi.Infrastructure;
using LearnWebApi.Repository;
using LearnWebApi.Services;
using LearnWebApi.Models;
using BookDataPump.Models;
using BookDataService.Service;
using BookDataPump.Framework;

namespace LearnWebApi
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
            services.AddSingleton<ILocalCache, LocalCache>()
            .AddSingleton<ParameterStore>()
            .AddTransient<IStrongPasswordRepository, StrongPasswordRepository>()
            .AddTransient<IStrongPasswordCheckService, StrongPasswordCheckService>()
            .AddTransient<IBookDataFetchService, BookDataFetchService>()
            .AddLogging()
            .AddDbContextPool<UserDataContext>(options => options.UseNpgsql(Configuration.GetSection("DBConnectionString").Value))
            .AddDbContext<BookItemsDbContext>(options => options.UseNpgsql(Configuration.GetSection("DBConnectionString").Value))
            .AddDbContext<UserDataContext>()
            .AddDbContext<BookItemsDbContext>()
            .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserDataContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.Migrate();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
