using Claims.Application.Interfaces;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Services
{
    public class ClaimService(IAuditService auditService, IClaimsContext dbContext, IGuidProvider guidProvider) : IClaimService
    {
        public async Task<Claim> AddClaimAsync(Claim claim)
        {
            claim.Id = guidProvider.NewStringGuid();

            dbContext.Claims.Add(claim);
            await dbContext.SaveChangesAsync();

            _ = Task.Run(() => auditService.AuditClaimAsync(claim.Id, Consts.HttpRequestTypePost));

            return claim;
        }

        public async Task DeleteClaimAsync(string claimId)
        {
            var claim = await dbContext.Claims.SingleOrDefaultAsync(x => x.Id == claimId);
            if (claim is null)
                throw new Exception($"Error when deleteing claim. Claim with id {claimId} does not exists");           

            dbContext.Claims.Remove(claim);
            await dbContext.SaveChangesAsync();

            _ = Task.Run(() => auditService.AuditClaimAsync(claimId, Consts.HttpRequestTypeDelete));            
        }

        public async Task<Claim> GetClaimAsync(string claimId)
        {
            var claim = await dbContext.Claims.SingleOrDefaultAsync(x => x.Id == claimId);
            if (claim is null)
                throw new Exception($"Claim with id {claimId} does not exists");

            return claim;
        }

        public async Task<List<Claim>> GetClaimsAsync()
        {
            return await dbContext.Claims.ToListAsync();
        }
    }
}
