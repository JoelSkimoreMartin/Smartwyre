using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Linq;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    public RebateServiceTests()
    {
        RebateDataStore = new Mock<IRebateDataStore>();
        ProductDataStore = new Mock<IProductDataStore>();
        RebateCalculatorFactory = new Mock<IRebateCalculatorFactory>();

        Func<string, Rebate> getRebate =
            (id) =>
            {
                Rebate rebate = null;

                if (int.TryParse(id, out int enumId))
                {
                    rebate =
                       new Rebate
                       {
                           Identifier = id,
                           Amount = 1m,
                           Incentive = (IncentiveType)enumId,
                           Percentage = 0.8m,
                       };
                }

                return rebate;
            };

        Func<string, Product> getProduct =
            (id) =>
            {
                Product product = null;

                if (int.TryParse(id, out var intId))
                {
                    var enumName = ((IncentiveType)intId).ToString();

                    product =
                       new Product
                       {
                           Id = intId,
                           Identifier = id,
                           Price = 1m,
                           SupportedIncentives = Enum.GetValues<SupportedIncentiveType>().FirstOrDefault(e => e.ToString() == enumName),
                           Uom = "Uom Test",
                       };
                }

                return product;
            };

        Func<Rebate, Product, IRebateCalculator> getCalculator =
            (rebate, product) =>
            {
                if (rebate.Incentive == IncentiveType.AmountPerUom &&
                    product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom))
                    return new AmountPerUom();

                if (rebate.Incentive == IncentiveType.FixedCashAmount &&
                    product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                    return new FixedCashAmount();

                if (rebate.Incentive == IncentiveType.FixedRateRebate &&
                    product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                    return new FixedRate();

                return null;
            };

        RebateDataStore
            .Setup(ds => ds.GetRebate(It.IsAny<string>()))
            .Returns(getRebate);

        ProductDataStore
            .Setup(ds => ds.GetProduct(It.IsAny<string>()))
            .Returns(getProduct);

        RebateCalculatorFactory
            .Setup(ds => ds.GetCalculator(It.IsAny<Rebate>(), It.IsAny<Product>()))
            .Returns(getCalculator);

        Instance =
            new RebateService(
                RebateDataStore.Object,
                ProductDataStore.Object,
                RebateCalculatorFactory.Object);
    }

    private RebateService Instance { get; }
    private Mock<IRebateDataStore> RebateDataStore { get; }
    private Mock<IProductDataStore> ProductDataStore { get; }
    private Mock<IRebateCalculatorFactory> RebateCalculatorFactory { get; }

    private
        (
        string request,
        string request_RebateIdentifier,
        string request_ProductIdentifier,
        string rebateDataStore,
        string productDataStore,
        string rebateCalculatorFactory
        )
        ParamName { get; } =
        (
        "request",
        "request.RebateIdentifier",
        "request.ProductIdentifier",
        "rebateDataStore",
        "productDataStore",
        "rebateCalculatorFactory"
        );

    #region Constructor

    [Fact]
    public void Constructor_Success()
    {
        var instance =
            new RebateService(
                RebateDataStore.Object,
                ProductDataStore.Object,
                RebateCalculatorFactory.Object);

        Assert.NotNull(instance);
    }

    [Fact]
    public void Constructor_Failure()
    {
        var exception1 = Assert.Throws<ArgumentNullException>(() => new RebateService(null, ProductDataStore.Object, RebateCalculatorFactory.Object));
        var exception2 = Assert.Throws<ArgumentNullException>(() => new RebateService(RebateDataStore.Object, null, RebateCalculatorFactory.Object));
        var exception3 = Assert.Throws<ArgumentNullException>(() => new RebateService(RebateDataStore.Object, ProductDataStore.Object, null));

        Assert.NotNull(exception1);
        Assert.NotNull(exception2);
        Assert.NotNull(exception3);
        Assert.Equal(ParamName.rebateDataStore, exception1.ParamName);
        Assert.Equal(ParamName.productDataStore, exception2.ParamName);
        Assert.Equal(ParamName.rebateCalculatorFactory, exception3.ParamName);
    }

    #endregion

    #region Calculate

    [Theory]
    [InlineData("0", "0", 1, true)]
    [InlineData("1", "1", 1, true)]
    [InlineData("2", "2", 1, true)]
    [InlineData("0", "0", 0, false)]
    [InlineData("1", "1", 0, false)]
    [InlineData("2", "2", 0, true)]
    [InlineData("3", "3", 1, false)]
    public void Calculate_Success(string rebateId, string productId, decimal volume, bool success)
    {
        var request =
            new CalculateRebateRequest
            {
                RebateIdentifier = rebateId,
                ProductIdentifier = productId,
                Volume = volume,
            };

        var result = Instance.Calculate(request);

        Assert.NotNull(result);
        Assert.Equal(success, result.Success);
    }

    [Fact]
    public void Calculate_Failure()
    {
        var exception1 = Assert.Throws<ArgumentNullException>(() => Instance.Calculate(null));
        var exception2 = Assert.Throws<ArgumentNullException>(() => Instance.Calculate(new CalculateRebateRequest { RebateIdentifier = "1" }));
        var exception3 = Assert.Throws<ArgumentNullException>(() => Instance.Calculate(new CalculateRebateRequest { ProductIdentifier = "1" }));

        Assert.NotNull(exception1);
        Assert.NotNull(exception2);
        Assert.NotNull(exception3);
        Assert.Equal(ParamName.request, exception1.ParamName);
        Assert.Equal(ParamName.request_ProductIdentifier, exception2.ParamName);
        Assert.Equal(ParamName.request_RebateIdentifier, exception3.ParamName);
    }

    #endregion
}