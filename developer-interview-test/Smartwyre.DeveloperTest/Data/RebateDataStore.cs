using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private List<Rebate> Rebates { get; } =
                            new(new[]
                                {
                                    new Rebate
                                    {
                                        Identifier = "1",
                                        Amount = 1.5m,
                                        Incentive = IncentiveType.FixedRateRebate,
                                        Percentage = 0.8m,
                                    },
                                    new Rebate
                                    {
                                        Identifier = "2",
                                        Amount = 1.5m,
                                        Incentive = IncentiveType.FixedCashAmount,
                                        Percentage = 0.8m,
                                    },
                                    new Rebate
                                    {
                                        Identifier = "3",
                                        Amount = 1.5m,
                                        Incentive = IncentiveType.AmountPerUom,
                                        Percentage = 0.8m,
                                    },
                                });

    public Rebate GetRebate(string rebateIdentifier) => Rebates.FirstOrDefault(r => r.Identifier == rebateIdentifier);

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}