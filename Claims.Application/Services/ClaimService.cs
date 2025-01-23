using Claims.Application.Interfaces;
using Claims.Core.Claims.Entities;
using Claims.Core.Claims.Interfaces;

namespace Claims.Application.Services;

public class ClaimService(IClaimsUnitOfWork unitOfWork) : IClaimService
{
    public async Task<Claim> AddClaimAsync(Claim claim)
    {
        await unitOfWork.Claims.AddAsync(claim);
        await unitOfWork.SaveAsync();

        return claim;
    }

    public async Task DeleteClaimAsync(string claimId)
    {
        var claim = await unitOfWork.Claims.GetByIdAsync(claimId);
        if (claim is null)
            throw new Exception($"Error when deleteing claim. Claim with id {claimId} does not exists");

        await unitOfWork.Claims.DeleteAsync(claimId);
        await unitOfWork.SaveAsync();
    }

    public async Task<Claim> GetClaimAsync(string claimId)
    {
        var claim = await unitOfWork.Claims.GetByIdAsync(claimId);
        if (claim is null)
            throw new Exception($"Claim with id {claimId} does not exists");

        return claim;
    }

    public async Task<List<Claim>> GetClaimsAsync()
    {
        return (await unitOfWork.Claims.GetAllAsync()).ToList();
    }
}
