using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.CommandLine;

namespace Smartwyre.DeveloperTest.Runner
{
    public class CommandLine : RootCommand
    {
        public class CalculateCommand : Command
        {
            public CalculateCommand(IRebateService rebateService)
                : base("calculate", "Calculate a rebate amount")
            {
                ArgumentNullException.ThrowIfNull(rebateService, nameof(rebateService));

                var options =
                    new
                    {
                        rebateId =
                            new Option<string>("--rebateId", description: "Identifier for a rebate")
                            {
                                IsRequired = true
                            },
                        productId =
                            new Option<string>("--productId", description: "Identifier for a product")
                            {
                                IsRequired = true
                            },
                        volume =
                            new Option<decimal>("--volume", description: "Volume for a rebate")
                            {
                                IsRequired = false
                            },
                    };

                Add(options.rebateId);
                Add(options.productId);
                Add(options.volume);

                this.SetHandler(
                        (rebateId, productId, volume) =>
                        {
                            var request =
                                new CalculateRebateRequest
                                {
                                    RebateIdentifier = rebateId,
                                    ProductIdentifier = productId,
                                    Volume = volume,
                                };

                            rebateService.Calculate(request);
                        },
                        options.rebateId,
                        options.productId,
                        options.volume);
            }
        }

        public CommandLine(CalculateCommand calculateCommand)
        {
            ArgumentNullException.ThrowIfNull(calculateCommand, nameof(calculateCommand));

            Name = "rebate";
            Description = "Smartwyre Developer Test: Rebate";

            Add(calculateCommand);
        }
    }
}