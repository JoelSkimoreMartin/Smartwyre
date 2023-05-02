using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.RebateCalculators
{
    public interface IRebateCalculatorFactory
    {
        IRebateCalculator GetCalculator(Rebate rebate, Product product);
    }
}