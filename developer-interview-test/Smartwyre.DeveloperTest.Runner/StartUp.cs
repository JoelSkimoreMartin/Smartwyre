using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using System;

namespace Smartwyre.DeveloperTest.Runner
{
    public class StartUp
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var startUp = new StartUp();
            var services = new ServiceCollection();

            startUp.ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            services
                .AddScoped(sp => sp)
                .AddSingleton<CommandLine>()
                .AddSingleton<CommandLine.CalculateCommand>()
                .AddInherited<IRebateCalculator>()
                .AddSmartwyreDeveloperTest();
        }
    }
}