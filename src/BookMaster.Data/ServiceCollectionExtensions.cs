using BookMaster.Data.Models;
using BookMaster.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookMaster.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBookDataDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            string DBServerUrl = configuration["DBServerUrl"];
            string DBUsername = configuration["DBUsername"];
            string DBPassword = configuration["DBPassword"];

            return services
                .AddDbContext<BookItemsDbContext>(options => 
                {
                    options.UseNpgsql($"Server={DBServerUrl};Database=Books;Username={DBUsername};Password={DBPassword}");
                })
                .AddTransient<BookRepository>();
        }
    }
}
