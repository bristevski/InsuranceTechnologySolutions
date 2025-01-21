using Claims.Application.Interfaces;
using Claims.Application.Providers.ComputingStrategy.Base;

namespace Claims.Application.Providers.ComputingStrategy
{
    public class YachtComputingStrategy : BaseComputingStrategy, IComputingStrategy
    {
        private readonly int IncreasedValuePercentagePerDay = 10;
        private readonly int DiscountFirst150Days = 5;
        private readonly int AdditionalDiscount = 3;
        private double DayRate => base.BaseDayRate + (base.BaseDayRate * IncreasedValuePercentagePerDay / 100);

        public decimal ComputePremium(DateTime startDate, DateTime endDate)
        {
            var totalDays = Math.Ceiling((endDate - startDate).TotalDays);

            return totalDays switch
            {
                <= 30  => base.ComputeFirst30Days(DayRate, totalDays),
                <= 180 => base.ComputeFirst180Days(DayRate, totalDays, DiscountFirst150Days),                
                _ => base.ComputeAllPeriod(DayRate, totalDays, DiscountFirst150Days, AdditionalDiscount)
            };
        }
    }
}
