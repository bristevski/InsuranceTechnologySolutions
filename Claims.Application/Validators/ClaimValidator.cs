using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Application.Validators
{
    public class ClaimValidator(ICoverService coverService) : IClaimValidator
    {
        public async Task<List<string>> ValidateModel(ClaimModel claimModel)
        {
            var errors = new List<string>();

            if (claimModel.DamageCost > 1000)
                errors.Add("Damage cost cannot exceed more than 1000");

            var cover = await coverService.GetCoverAsync(claimModel.CoverId);
            if (claimModel.Created < cover.StartDate || claimModel.Created > cover.EndDate)
                errors.Add("Claim is created outside of insurance period");


            return errors;
        }
    }
}
