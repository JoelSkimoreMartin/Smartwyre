using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

/// <inheritdoc cref="IRebateService" />
public class RebateService : IRebateService
{
    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IRebateCalculatorFactory rebateCalculatorFactory)
    {
        ArgumentNullException.ThrowIfNull(rebateDataStore, nameof(rebateDataStore));
        ArgumentNullException.ThrowIfNull(productDataStore, nameof(productDataStore));
        ArgumentNullException.ThrowIfNull(rebateCalculatorFactory, nameof(rebateCalculatorFactory));

        RebateCalculatorFactory = rebateCalculatorFactory;

        DataStore = (rebateDataStore, productDataStore);
    }

    private IRebateCalculatorFactory RebateCalculatorFactory { get; }
    private (IRebateDataStore Rebate, IProductDataStore Product) DataStore { get; }

    /// <inheritdoc />
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var rebate = DataStore.Rebate.GetRebate(request.RebateIdentifier);
        if (rebate is null)
            return CalculateRebateResult.Failed;

        var product = DataStore.Product.GetProduct(request.ProductIdentifier);
        if (product is null)
            return CalculateRebateResult.Failed;

        var calculator = RebateCalculatorFactory.GetCalculator(rebate, product);
        if (calculator is null)
            return CalculateRebateResult.Failed;

        var calcRequest =
            new RebateCalculationRequest
            {
                RebateRequest = request,
                Rebate = rebate,
                Product = product,
            };

        if (calculator.IsValid(calcRequest) == false)
            return CalculateRebateResult.Failed;

        var rebateAmount = calculator.CalculateAmount(calcRequest);

        DataStore.Rebate.StoreCalculationResult(calcRequest.Rebate, rebateAmount);

        return CalculateRebateResult.Succeeded;
    }
}