using Smartwyre.DeveloperTest;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartwyreDeveloperTest(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            return
                services
                    .AddScoped<IRebateService, RebateService>()
                    .AddScoped<IRebateDataStore, RebateDataStore>()
                    .AddScoped<IProductDataStore, ProductDataStore>()
                    .AddScoped<IRebateCalculatorFactory, RebateCalculatorFactory>();
        }

        public static IServiceCollection AddInherited<TType>(this IServiceCollection services)
        {
            var type = typeof(TType);

            var mappings = type.InterfaceMappings();

            foreach (var map in mappings)
            {
                services.AddScoped(map.ServiceType, map.ImplementationType);
            }

            return services;
        }
    }
}