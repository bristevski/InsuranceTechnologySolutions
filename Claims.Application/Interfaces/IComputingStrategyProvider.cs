using Claims.Core.Claims.Entities.Enums;

namespace Claims.Application.Interfaces
{
    public interface IComputingStrategyProvider
    {
        decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType type);
    }
}
