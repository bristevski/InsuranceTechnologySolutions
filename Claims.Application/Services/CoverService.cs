using Claims.Application.Interfaces;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;

namespace Claims.Application.Services
{
    public class CoverService(IClaimsUnitOfWork unitOfWork, IComputingStrategyProvider computingStrategyProvider) : ICoverService
    {
        public async Task<Cover> AddCoverAsync(Cover cover)
        {
            cover.Premium = ComputePremium(cover);

            await unitOfWork.Covers.AddAsync(cover);
            await unitOfWork.SaveAsync();

            return cover;
        }

        public async Task DeleteCoverAsync(string coverId)
        {            
            var cover = await unitOfWork.Covers.GetByIdAsync(coverId);
            if (cover is null)
                throw new Exception($"Error when delete cover. Cover with id {coverId} does not exists");

            await unitOfWork.Covers.DeleteAsync(coverId);
            await unitOfWork.SaveAsync();
        }

        public async Task<Cover> GetCoverAsync(string coverId)
        {
            var cover = await unitOfWork.Covers.GetByIdAsync(coverId);
            if (cover is null)
                throw new Exception($"Cover with id {coverId} does not exists");

            return cover;
        }

        public async Task<List<Cover>> GetCoversAsync()
        {
            return (await unitOfWork.Covers.GetAllAsync()).ToList();
        }

        public decimal ComputePremium(Cover cover)
        {
            return computingStrategyProvider.ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        }
    }
}
