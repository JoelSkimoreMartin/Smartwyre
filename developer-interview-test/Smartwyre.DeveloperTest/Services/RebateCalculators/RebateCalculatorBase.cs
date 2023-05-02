using System;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public abstract class RebateCalculatorBase : IRebateCalculator
    {
        protected abstract IncentiveType IncentiveType { get; }
        protected abstract SupportedIncentiveType SupportedIncentive { get; }

        public virtual decimal CalculateAmount(RebateCalculationRequest request)
        {
            if (IsValid(request) == false)
                throw new ArgumentException("Invalid request", paramName: nameof(request));

            return 0m;
        }

        public bool IsSupported(RebateCalculationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(request.Rebate, $"{nameof(request)}.{nameof(request.Rebate)}");
            ArgumentNullException.ThrowIfNull(request.Product, $"{nameof(request)}.{nameof(request.Product)}");

            return
                request.Rebate.Incentive == IncentiveType
                &&
                request.Product.SupportedIncentives.HasFlag(SupportedIncentive);
        }

        public virtual bool IsValid(RebateCalculationRequest request) => IsSupported(request);
    }
}