using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.RebateCalculators;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private (string request, string rebateDataStore, string productDataStore, string rebateCalculatorFactory) ParamName { get; } =
            ("request", "rebateDataStore", "productDataStore", "rebateCalculatorFactory");

    [Fact]
    public void Constructor_Success()
    {
        var rebateDataStore = new Mock<IRebateDataStore>();
        var productDataStore = new Mock<IProductDataStore>();
        var rebateCalculatorFactory = new Mock<IRebateCalculatorFactory>();

        var instance =
            new RebateService(
                rebateDataStore.Object,
                productDataStore.Object,
                rebateCalculatorFactory.Object);

        Assert.NotNull(instance);
    }

    [Fact]
    public void Constructor_Failure()
    {
    }
}