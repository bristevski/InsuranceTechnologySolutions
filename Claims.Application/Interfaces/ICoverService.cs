using Claims.Application.Models.Enums;
using Core.Claims.Entities;

namespace Claims.Application.Interfaces
{
    public interface ICoverService
    {
        Task<List<Cover>> GetCoversAsync();
        Task<Cover> GetCoverAsync(string coverId);
        Task<Cover> AddCoverAsync(Cover cover);
        Task DeleteCoverAsync(string coverId);
        decimal ComputePremium(DateTime startDate, DateTime endDate, CoverModelType type);
    }
}
