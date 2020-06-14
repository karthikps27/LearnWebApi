using BookDataPump.Framework;
using BookDataPump.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookDataPump
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBookDataDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            string DBServerUrl = configuration["DBServerUrl"];
            string DBUsername = configuration["DBUsername"];
            string DBPassword = configuration["DBPassword"];

            return services
                .AddDbContext<BookItemsDbContext>(options => options.UseNpgsql($"Server={DBServerUrl};Database=Books;User Id={DBUsername}Password={DBPassword}"));
        }
    }
}
