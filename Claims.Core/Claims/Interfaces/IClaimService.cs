using Core.Claims.Entities;

namespace Claims.Core.Claims.Interfaces
{
    public interface IClaimService
    {
        Task<List<Claim>> GetClaimsAsync();
        Task<Claim> GetClaimAsync(string claimId);
        Task<Claim> AddClaimAsync(Claim claim);
        Task DeleteClaimAsync(string claimId);
    }
}
