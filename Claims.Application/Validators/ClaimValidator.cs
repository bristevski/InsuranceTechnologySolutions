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
                errors.Add(ClaimErrorMessages.DamageCostTooHigh);

            var cover = await coverService.GetCoverAsync(claimModel.CoverId);
            if (claimModel.Created < cover.StartDate || claimModel.Created > cover.EndDate)
                errors.Add(ClaimErrorMessages.InvalidCreationDate);


            return errors;
        }
    }
}
