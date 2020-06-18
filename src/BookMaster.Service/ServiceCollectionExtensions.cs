using BookDataService.Service;
using BookMaster.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookMaster.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                 .AddTransient<S3FileProcessorService>()
                 .AddTransient<BookManagerService>();
        }
    }
}
