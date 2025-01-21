using Claims.Application.Interfaces;
using Claims.Core.Audit.Interfaces;
using Claims.Core.Claims.Interfaces;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Services
{
    public class ClaimService(IAuditService auditService, IClaimsContext dbContext, IGuidProvider guidProvider) : IClaimService
    {
        private IClaimsContext _dbContext { get; } = dbContext;
        private IAuditService _auditService { get; } = auditService;
        private IGuidProvider _guidProvider { get; } = guidProvider;

        public async Task<Claim> AddClaimAsync(Claim claim)
        {
            claim.Id = _guidProvider.NewStringGuid();

            _dbContext.Claims.Add(claim);
            await _dbContext.SaveChangesAsync();

            _auditService.AuditCover(claim.Id, Consts.HttpRequestTypePost);

            return claim;
        }

        public async Task DeleteClaimAsync(string claimId)
        {
            var claim = _dbContext.Claims.FirstOrDefault(x => x.Id == claimId);
            if (claim is null)
                throw new Exception($"Error when delete claim. Claim with id {claimId} does not exists");           

            _dbContext.Claims.Remove(claim);
            await _dbContext.SaveChangesAsync();

            _auditService.AuditCover(claimId, Consts.HttpRequestTypeDelete);
        }

        public async Task<Claim> GetClaimAsync(string claimId)
        {
            return await _dbContext.Claims.FirstOrDefaultAsync(x => x.Id == claimId);
        }

        public async Task<List<Claim>> GetClaimsAsync()
        {
            return await _dbContext.Claims.ToListAsync();
        }
    }
}
