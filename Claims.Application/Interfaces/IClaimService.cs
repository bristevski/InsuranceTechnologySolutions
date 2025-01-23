using Claims.Core.Claims.Entities;

namespace Claims.Application.Interfaces;

public interface IClaimService
{
    Task<List<Claim>> GetClaimsAsync();
    Task<Claim> GetClaimAsync(string claimId);
    Task<Claim> AddClaimAsync(Claim claim);
    Task DeleteClaimAsync(string claimId);
}
