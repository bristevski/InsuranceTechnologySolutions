using Claims.Application.Interfaces;
using Claims.Application.Providers.ComputingStrategy.Base;

namespace Claims.Application.Providers.ComputingStrategy;

public class ContainerShipComputingStrategy : BaseComputingStrategy, IComputingStrategy
{
    private readonly int _increasedValuePercentagePerDay = 30;
    private readonly int _discountFirst150Days = 2;
    private readonly int _additionalDiscount = 1;
    private double _dayRate => base._baseDayRate + (base._baseDayRate * _increasedValuePercentagePerDay / 100);

    public decimal ComputePremium(DateTime startDate, DateTime endDate)
    {
        var totalDays = Math.Ceiling((endDate - startDate).TotalDays);

        return totalDays switch
        {
            <= 30 => base.ComputeFirst30Days(_dayRate, totalDays),
            <= 180 => base.ComputeFirst180Days(_dayRate, totalDays, _discountFirst150Days),
            _ => base.ComputeAllPeriod(_dayRate, totalDays, _discountFirst150Days, _additionalDiscount)
        };
    }
}
