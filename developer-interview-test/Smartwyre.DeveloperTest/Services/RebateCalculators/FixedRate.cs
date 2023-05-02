using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IFixedRate : IRebateCalculator { }

    public class FixedRate : RebateCalculatorBase, IFixedRate
    {
        protected override IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
        protected override SupportedIncentiveType SupportedIncentive => SupportedIncentiveType.FixedRateRebate;

        public override bool IsValid(RebateCalculationRequest request)
        {
            if (base.IsValid(request) == false)
                return false;

            ArgumentNullException.ThrowIfNull(request.RebateRequest, $"{nameof(request)}.{nameof(request.RebateRequest)}");

            return
                request.Rebate.Percentage != 0
                &&
                request.Product.Price != 0
                &&
                request.RebateRequest.Volume != 0;
        }

        public override decimal CalculateAmount(RebateCalculationRequest request) =>
                           base.CalculateAmount(request)
                                + request.Product.Price * request.Rebate.Percentage * request.RebateRequest.Volume;
    }
}