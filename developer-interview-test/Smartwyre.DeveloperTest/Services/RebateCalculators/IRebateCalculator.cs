using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IRebateCalculator
    {
        bool IsValid(RebateCalculationRequest request);
        bool IsSupported(RebateCalculationRequest request);
        decimal CalculateAmount(RebateCalculationRequest request);
    }
}