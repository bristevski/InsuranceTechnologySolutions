using Claims.Application.Interfaces;
using Claims.Application.Models;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController(IClaimService claimService, IClaimValidator validator) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ClaimModel>> GetAsync()
        {
            return (await claimService.GetClaimsAsync()).Select(x => new ClaimModel(x));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ClaimModel claimModel)
        {
            var errors = await validator.ValidateModel(claimModel);
            if(errors.Any())
                return BadRequest(errors);

            var claim = await claimService.AddClaimAsync(claimModel.ToDomainModel());
            claim.Id = claim.Id;
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            await claimService.DeleteClaimAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ClaimModel> GetAsync(string id)
        {
            return new ClaimModel(await claimService.GetClaimAsync(id));
        }
    }
}
