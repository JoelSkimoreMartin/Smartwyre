using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public class RebateCalculatorFactory : IRebateCalculatorFactory
    {
        public RebateCalculatorFactory(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

            ServiceProvider = serviceProvider;
        }

        private IServiceProvider ServiceProvider { get; }

        private Dictionary<Type, bool> Interfaces { get; } = typeof(IRebateCalculator).InheritedInterfaces().ToDictionary(i => i, i => false);
        private Dictionary<IncentiveType, Dictionary<SupportedIncentiveType, Type>> Mapping { get; } = new();

        public IRebateCalculator GetCalculator(Rebate rebate, Product product)
        {
            ArgumentNullException.ThrowIfNull(rebate, nameof(rebate));
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            if (Mapping.TryGetValue(rebate.Incentive, out var supported) == false)
            {
                Mapping[rebate.Incentive] = supported = new();
            }
            else
                if (supported.TryGetValue(product.SupportedIncentives, out var serviceType))
                    return ServiceProvider.GetService(serviceType) as IRebateCalculator;

            var request =
                new RebateCalculationRequest
                {
                    Rebate = rebate,
                    Product = product,
                };

            foreach (var @interface in Interfaces.Keys)
            {
                if (Interfaces[@interface])
                    continue;

                var calculator = ServiceProvider.GetService(@interface) as IRebateCalculator;

                if (calculator.IsSupported(request) == false)
                    continue;

                Interfaces[@interface] = true;
                supported[product.SupportedIncentives] = @interface;

                return calculator;
            }

            return null;
        }
    }
}