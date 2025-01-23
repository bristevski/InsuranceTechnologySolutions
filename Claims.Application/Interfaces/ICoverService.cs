using Claims.Core.Claims.Entities;

namespace Claims.Application.Interfaces;

public interface ICoverService
{
    Task<List<Cover>> GetCoversAsync();
    Task<Cover> GetCoverAsync(string coverId);
    Task<Cover> AddCoverAsync(Cover cover);
    Task DeleteCoverAsync(string coverId);
    decimal ComputePremium(Cover cover);
}
