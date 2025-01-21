using Claims.Application.Interfaces;
using Claims.Application.Providers;
using Claims.Core.Audit.Interfaces;
using Claims.Core.Claims.Interfaces;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Services
{
    public class CoverService(IAuditService auditService, IClaimsContext dbContext, IComputingStrategyProvider computingStrategyProvider, IGuidProvider guidProvider) : ICoverService
    {
        private IAuditService _auditService { get; } = auditService;
        private IClaimsContext _dbContext { get; } = dbContext;
        private IComputingStrategyProvider _computingStrategyProvider { get; } = computingStrategyProvider;
        private IGuidProvider _guidProvider { get; } = guidProvider;

        public async Task<Cover> AddCoverAsync(Cover cover)
        {
            cover.Id = _guidProvider.NewStringGuid();

            var computingStrategy = _computingStrategyProvider.GetComputingStrategy(cover.Type);
            cover.Premium = computingStrategy.ComputePremium(cover.StartDate, cover.EndDate);

            _dbContext.Covers.Add(cover);
            await _dbContext.SaveChangesAsync();

            _auditService.AuditCover(cover.Id, Consts.HttpRequestTypePost);

            return cover;
        }

        public async Task DeleteCoverAsync(string coverId)
        {            
            var cover = await _dbContext.Covers.FirstOrDefaultAsync(x => x.Id == coverId);
            if (cover is null)
                throw new Exception($"Error when delete claim. Claim with id {coverId} does not exists");           

            _dbContext.Covers.Remove(cover);
            await _dbContext.SaveChangesAsync();

            _auditService.AuditCover(coverId, Consts.HttpRequestTypeDelete);

        }

        public async Task<Cover> GetCoverAsync(string coverId)
        {
            return await _dbContext.Covers.FirstOrDefaultAsync(x => x.Id == coverId);
        }

        public async Task<List<Cover>> GetCoversAsync()
        {
            return await _dbContext.Covers.ToListAsync();
        }
    }
}
