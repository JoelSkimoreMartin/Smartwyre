using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IAmountPerUom : IRebateCalculator { }

    public class AmountPerUom : RebateCalculatorBase, IAmountPerUom
    {
        protected override IncentiveType IncentiveType => IncentiveType.AmountPerUom;
        protected override SupportedIncentiveType SupportedIncentive => SupportedIncentiveType.AmountPerUom;

        public override bool IsValid(RebateCalculationRequest request)
        {
            if (base.IsValid(request) == false)
                return false;

            ArgumentNullException.ThrowIfNull(request.RebateRequest, $"{nameof(request)}.{nameof(request.RebateRequest)}");

            return
                request.Rebate.Amount != 0
                &&
                request.RebateRequest.Volume != 0;
        }

        public override decimal CalculateAmount(RebateCalculationRequest request) =>
                           base.CalculateAmount(request)
                                + request.Rebate.Amount * request.RebateRequest.Volume;
    }
}