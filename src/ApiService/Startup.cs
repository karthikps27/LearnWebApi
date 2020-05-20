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
            .AddTransient<IStrongPasswordRepository, StrongPasswordRepository>()
            .AddTransient<IStrongPasswordCheckService, StrongPasswordCheckService>()
            .AddTransient<IBookDataFetchService, BookDataFetchService>()
            .AddDbContextPool<UserDataContext>(options => options.UseNpgsql(Configuration.GetSection("DBConnectionString").Value))
            .AddDbContext<BookItemsDbContext>(options => options.UseNpgsql(Configuration.GetSection("DBConnectionString").Value))
            .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
