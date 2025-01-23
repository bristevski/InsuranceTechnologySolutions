using Claims.Application.Models;

namespace Claims.Application.Interfaces;

public interface IClaimValidator
{
    Task<List<string>> ValidateModel(ClaimModel claimModel);
}
