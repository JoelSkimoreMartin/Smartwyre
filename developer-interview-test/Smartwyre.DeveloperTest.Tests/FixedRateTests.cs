using Smartwyre.DeveloperTest.Services.RebateCalculators;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedRateTests
{
    private FixedRate Instance { get; } = new FixedRate();

    private
        (
        string request,
        string request_Rebate,
        string request_Product,
        string request_RebateRequest
        )
        ParamName { get; } =
        (
        "request",
        "request.Rebate",
        "request.Product",
        "request.RebateRequest"
        );

    #region IsSupported

    [Theory]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedCashAmount, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedRateRebate, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.AmountPerUom, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedCashAmount, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedRateRebate, false)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.AmountPerUom, false)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedCashAmount, false)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedRateRebate, true)]
    public void IsSupported_Success(IncentiveType incentive, SupportedIncentiveType supported, bool isSupported)
    {
        var request =
            new RebateCalculationRequest
            {
                Rebate = new Rebate { Incentive = incentive },
                Product = new Product { SupportedIncentives = supported },
            };

        var response = Instance.IsSupported(request);

        Assert.Equal(response, isSupported);
    }

    [Fact]
    public void IsSupported_Failure()
    {
        var exception1 = Assert.Throws<ArgumentNullException>(() => Instance.IsSupported(null));
        var exception2 = Assert.Throws<ArgumentNullException>(() => Instance.IsSupported(new RebateCalculationRequest { Product = new() }));
        var exception3 = Assert.Throws<ArgumentNullException>(() => Instance.IsSupported(new RebateCalculationRequest { Rebate = new() }));

        Assert.NotNull(exception1);
        Assert.NotNull(exception2);
        Assert.NotNull(exception3);
        Assert.Equal(ParamName.request, exception1.ParamName);
        Assert.Equal(ParamName.request_Rebate, exception2.ParamName);
        Assert.Equal(ParamName.request_Product, exception3.ParamName);
    }

    #endregion

    #region IsValid

    [Theory]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedRateRebate, 1, 1, 1, true)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 1, 1, 1, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 0, 0, 0, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 1, 1, 0, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 0, 0, 1, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedCashAmount, 1, 1, 1, false)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedRateRebate, 1, 1, 1, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.AmountPerUom, 1, 1, 1, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedCashAmount, 1, 1, 1, false)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedRateRebate, 1, 1, 1, false)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.AmountPerUom, 1, 1, 1, false)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedCashAmount, 1, 1, 1, false)]
    public void IsValid_Success(IncentiveType incentive, SupportedIncentiveType supported, decimal percentage, decimal price, decimal volume, bool isSupported)
    {
        var request =
            new RebateCalculationRequest
            {
                Rebate = new Rebate { Percentage = percentage, Incentive = incentive },
                Product = new Product { Price = price, SupportedIncentives = supported },
                RebateRequest = new CalculateRebateRequest { Volume = volume },
            };

        var response = Instance.IsValid(request);

        Assert.Equal(response, isSupported);
    }

    [Fact]
    public void IsValid_Failure()
    {
        var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate };
        var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate };

        var exception1 = Assert.Throws<ArgumentNullException>(() => Instance.IsValid(null));
        var exception2 = Assert.Throws<ArgumentNullException>(() => Instance.IsValid(new RebateCalculationRequest { Product = product, RebateRequest = new() }));
        var exception3 = Assert.Throws<ArgumentNullException>(() => Instance.IsValid(new RebateCalculationRequest { Rebate = rebate, RebateRequest = new() }));
        var exception4 = Assert.Throws<ArgumentNullException>(() => Instance.IsValid(new RebateCalculationRequest { Product = product, Rebate = rebate }));

        Assert.NotNull(exception1);
        Assert.NotNull(exception2);
        Assert.NotNull(exception3);
        Assert.NotNull(exception4);
        Assert.Equal(ParamName.request, exception1.ParamName);
        Assert.Equal(ParamName.request_Rebate, exception2.ParamName);
        Assert.Equal(ParamName.request_Product, exception3.ParamName);
        Assert.Equal(ParamName.request_RebateRequest, exception4.ParamName);
    }

    #endregion

    #region CalculateAmount

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 1, 2, 2)]
    [InlineData(2, 2, 2, 8)]
    public void CalculateAmount_Success(decimal percentage, decimal price, decimal volume, decimal calculated)
    {
        var request =
            new RebateCalculationRequest
            {
                Rebate = new Rebate { Percentage = percentage, Incentive = IncentiveType.FixedRateRebate },
                Product = new Product { Price = price, SupportedIncentives = SupportedIncentiveType.FixedRateRebate },
                RebateRequest = new CalculateRebateRequest { Volume = volume },
            };

        var response = Instance.CalculateAmount(request);

        Assert.Equal(response, calculated);
    }

    [Theory]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 0, 0)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 1, 0)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.AmountPerUom, 0, 1)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedCashAmount, 1, 1)]
    [InlineData(IncentiveType.AmountPerUom, SupportedIncentiveType.FixedRateRebate, 1, 1)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.AmountPerUom, 1, 1)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedCashAmount, 1, 1)]
    [InlineData(IncentiveType.FixedCashAmount, SupportedIncentiveType.FixedRateRebate, 1, 1)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.AmountPerUom, 1, 1)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedCashAmount, 1, 1)]
    [InlineData(IncentiveType.FixedRateRebate, SupportedIncentiveType.FixedRateRebate, 1, 1)]
    public void CalculateAmount_Failure(IncentiveType incentive, SupportedIncentiveType supported, decimal amount, decimal volume)
    {
        var request =
            new RebateCalculationRequest
            {
                Rebate = new Rebate { Amount = amount, Incentive = incentive },
                Product = new Product { SupportedIncentives = supported },
                RebateRequest = new CalculateRebateRequest { Volume = volume },
            };

        var exception = Assert.Throws<ArgumentException>(() => Instance.CalculateAmount(request));

        Assert.NotNull(exception);
        Assert.NotNull(exception.Message);
        Assert.Equal(ParamName.request, exception.ParamName);
    }

    #endregion
}