namespace Claims.Application.Providers.ComputingStrategy.Base
{
    public abstract class BaseComputingStrategy
    {
        public readonly int BaseDayRate = 1250;

        public decimal ComputeFirst30Days(double dayRate, double totalDays)
        {
            return Convert.ToDecimal(dayRate * totalDays);
        }

        public decimal ComputeFirst180Days(double dayRate, double totalDays, int discountFirst150Days)
        {
            decimal total = 0;
            total += ComputeFirst30Days(dayRate, 30);

            dayRate = dayRate - dayRate * discountFirst150Days / 100;
            total += Convert.ToDecimal((totalDays - 30) * dayRate); 

            return total;
        }

        public decimal ComputeAllPeriod(double dayRate, double totalDays, int discountFirst150Days, int additionalDiscount)
        {
            decimal total = 0;
            total += ComputeFirst180Days(dayRate, 180, discountFirst150Days);

            dayRate = dayRate - dayRate * additionalDiscount / 100;
            total += Convert.ToDecimal((totalDays - 180) * dayRate);

            return total;
        }
    }
}
