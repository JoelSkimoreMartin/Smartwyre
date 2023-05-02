using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IAmountPerUom : IRebateCalculator { }

    public class AmountPerUom : RebateCalculatorBase, IAmountPerUom
    {
        protected override IncentiveType IncentiveType => IncentiveType.AmountPerUom;
        protected override SupportedIncentiveType SupportedIncentive => SupportedIncentiveType.AmountPerUom;

        public override bool IsValid(RebateCalculationRequest request) =>
            base.IsValid(request)
            &&
            request.Rebate.Amount != 0
            &&
            request.RebateRequest.Volume != 0;

        public override decimal CalculateAmount(RebateCalculationRequest request)
        {
            return base.CalculateAmount(request) + request.Rebate.Amount * request.RebateRequest.Volume;
        }
    }
}