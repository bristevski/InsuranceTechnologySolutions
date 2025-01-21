using Claims.Core.Claims.Entities.Enums;

namespace Claims.Application.Interfaces
{
    public interface IComputingStrategyProvider
    {
        IComputingStrategy GetComputingStrategy(CoverType type);
    }
}
