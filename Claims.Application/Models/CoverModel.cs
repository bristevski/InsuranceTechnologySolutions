using Claims.Application.Models.Enums;

namespace Claims.Application.Models
{
    public record CoverModel(string Id, DateTime StartDate, DateTime EndDate, CoverModelType Type, decimal Premium);
}
