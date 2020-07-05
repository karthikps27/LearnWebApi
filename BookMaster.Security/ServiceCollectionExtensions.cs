using BookMaster.Security.Framework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookMaster.Security
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurityServices(this IServiceCollection services)
        {
            return services
                 .AddTransient<AwsParameterStore>()
                 .AddTransient<TokenManagerService>();
        }
    }
}
