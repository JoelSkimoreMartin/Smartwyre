namespace Smartwyre.DeveloperTest.Types
{
    public class RebateCalculationRequest
    {
        public Rebate Rebate { get; set; }
        public Product Product { get; set; }
        public CalculateRebateRequest RebateRequest { get; set; }
    }
}