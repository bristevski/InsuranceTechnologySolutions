using Claims.Core.Claims.Entities.Enums;
namespace Claims.Application.Interfaces
{
    public interface IComputingStrategy
    {
        decimal ComputePremium(DateTime startDate, DateTime endDate);
    }
}
