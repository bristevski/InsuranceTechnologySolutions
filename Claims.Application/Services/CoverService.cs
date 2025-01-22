using Claims.Application.Interfaces;
using Claims.Application.Models.Enums;
using Claims.Core.Claims.Entities.Enums;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Services
{
    public class CoverService(IClaimsContext dbContext, IComputingStrategyProvider computingStrategyProvider) : ICoverService
    {
        public async Task<Cover> AddCoverAsync(Cover cover)
        {
            var computingStrategy = computingStrategyProvider.GetComputingStrategy(cover.Type);
            cover.Premium = computingStrategy.ComputePremium(cover.StartDate, cover.EndDate);

            dbContext.Covers.Add(cover);
            await dbContext.SaveChangesAsync();

            return cover;
        }

        public async Task DeleteCoverAsync(string coverId)
        {            
            var cover = await dbContext.Covers.SingleOrDefaultAsync(x => x.Id == coverId);
            if (cover is null)
                throw new Exception($"Error when delete cover. Cover with id {coverId} does not exists");           

            dbContext.Covers.Remove(cover);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Cover> GetCoverAsync(string coverId)
        {
            var cover =  await dbContext.Covers.SingleOrDefaultAsync(x => x.Id == coverId);
            if (cover is null)
                throw new Exception($"Cover with id {coverId} does not exists");

            return cover;
        }

        public async Task<List<Cover>> GetCoversAsync()
        {
            return await dbContext.Covers.ToListAsync();
        }

        public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverModelType type)
        {
            var computingStrategy = computingStrategyProvider.GetComputingStrategy((CoverType)type);
            return computingStrategy.ComputePremium(startDate, endDate);
        }
    }
}
