using Claims.Application.Models.Enums;

namespace Claims.Application.Models
{
    public record ClaimModel(string Id, string CoverId, DateTime Created, string Name, ClaimModelType Type, decimal DamageCost);
}
