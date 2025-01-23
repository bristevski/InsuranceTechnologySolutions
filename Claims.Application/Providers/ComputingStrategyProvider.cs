using Claims.Application.Interfaces;
using Claims.Application.Providers.ComputingStrategy;
using Claims.Core.Claims.Entities.Enums;

namespace Claims.Application.Providers;

public class ComputingStrategyProvider : IComputingStrategyProvider
{
    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        IComputingStrategy computingStrategy = coverType switch
        {
            CoverType.Yacht => new YachtComputingStrategy(),
            CoverType.PassengerShip => new PassengerShipComputingStrategy(),
            CoverType.ContainerShip => new ContainerShipComputingStrategy(),
            CoverType.BulkCarrier => new BulkCarrierComputingStrategy(),
            CoverType.Tanker => new TankerComputingStrategy(),
            _ => throw new Exception($"Type {Convert.ToString(coverType)} does not exists!")
        };

        return computingStrategy.ComputePremium(startDate, endDate);
    }
}
