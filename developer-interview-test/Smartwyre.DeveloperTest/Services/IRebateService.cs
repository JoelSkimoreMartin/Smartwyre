using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

/// <summary>
/// Service for managing rebates
/// </summary>
public interface IRebateService
{
    /// <summary>
    /// Calculate a rebate and store the results in the database
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    CalculateRebateResult Calculate(CalculateRebateRequest request);
}