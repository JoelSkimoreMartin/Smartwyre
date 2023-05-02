namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public static CalculateRebateResult Failed => new(false);
    public static CalculateRebateResult Succeeded => new(true);

    private CalculateRebateResult(bool success)
    {
        Success = success;
    }

    public bool Success { get; }
}