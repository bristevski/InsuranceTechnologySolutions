using Core.Claims.Entities;

namespace Claims.Core.Claims.Interfaces
{
    public interface ICoverService
    {
        Task<List<Cover>> GetCoversAsync();
        Task<Cover> GetCoverAsync(string coverId);
        Task<Cover> AddCoverAsync(Cover cover);
        Task DeleteCoverAsync(string coverId);
    }
}
