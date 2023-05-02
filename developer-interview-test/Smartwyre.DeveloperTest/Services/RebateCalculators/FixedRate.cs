using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IFixedRate : IRebateCalculator { }

    public class FixedRate : RebateCalculatorBase, IFixedRate
    {
        protected override IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
        protected override SupportedIncentiveType SupportedIncentive => SupportedIncentiveType.FixedRateRebate;

        public override bool IsValid(RebateCalculationRequest request) =>
            base.IsValid(request)
            &&
            request.Rebate.Percentage != 0
            &&
            request.Product.Price != 0
            &&
            request.RebateRequest.Volume != 0;

        public override decimal CalculateAmount(RebateCalculationRequest request)
        {
            return base.CalculateAmount(request) + request.Product.Price * request.Rebate.Percentage * request.RebateRequest.Volume;
        }
    }
}