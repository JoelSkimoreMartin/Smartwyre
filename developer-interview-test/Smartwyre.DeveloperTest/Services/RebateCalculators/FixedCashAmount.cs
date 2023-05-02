using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IFixedCashAmount : IRebateCalculator { }

    public class FixedCashAmount : RebateCalculatorBase, IFixedCashAmount
    {
        protected override IncentiveType IncentiveType => IncentiveType.FixedCashAmount;
        protected override SupportedIncentiveType SupportedIncentive => SupportedIncentiveType.FixedCashAmount;

        public override bool IsValid(RebateCalculationRequest request) =>
            base.IsValid(request)
            &&
            request.Rebate.Amount != 0;

        public override decimal CalculateAmount(RebateCalculationRequest request)
        {
            return base.CalculateAmount(request) + request.Rebate.Amount;
        }
    }
}