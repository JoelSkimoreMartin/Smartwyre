using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    private List<Product> Products { get; } =
                            new(new[]
                                {
                                    new Product
                                    {
                                        Id = 1,
                                        Identifier = "1",
                                        Price = 100m,
                                        SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                                    },
                                    new Product
                                    {
                                        Id = 2,
                                        Identifier = "2",
                                        Price = 100m,
                                        SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                                    },
                                    new Product
                                    {
                                        Id = 3,
                                        Identifier = "3",
                                        Price = 100m,
                                        SupportedIncentives = SupportedIncentiveType.AmountPerUom,
                                    },
                                });

    public Product GetProduct(string productIdentifier) => Products.FirstOrDefault(r => r.Identifier == productIdentifier);
}