using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public class RebateCalculatorFactory : IRebateCalculatorFactory
    {
        public RebateCalculatorFactory(List<IRebateCalculator> rebateCalculators)
        {
            ArgumentNullException.ThrowIfNull(rebateCalculators, nameof(rebateCalculators));

            RebateCalculators = rebateCalculators;
        }

        private List<IRebateCalculator> RebateCalculators { get; }

        public IRebateCalculator GetCalculator(Rebate rebate, Product product)
        {
            ArgumentNullException.ThrowIfNull(rebate, nameof(rebate));
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            var request =
                new RebateCalculationRequest
                {
                    Rebate = rebate,
                    Product = product,
                };

            return RebateCalculators.FirstOrDefault(rc => rc.IsSupported(request));
        }
    }
}