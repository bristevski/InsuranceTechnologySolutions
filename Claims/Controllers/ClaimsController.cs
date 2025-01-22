using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController(IClaimService claimService, IAuditService auditService, IGuidProvider guidProvider, IClaimValidator validator) : ControllerBase
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

            claimModel.Id = guidProvider.NewStringGuid();

            var addClaimTask = claimService.AddClaimAsync(claimModel.ToDomainModel());
            var addAuditTask = auditService.AuditCoverAsync(claimModel.Id, Consts.HttpRequestTypePost);

            Task.WaitAll(addClaimTask, addAuditTask);

            return Ok(claimModel);
        }

        [HttpDelete("{id}")]
        public Task<ActionResult> DeleteAsync(string id)
        {
            var deleteClaimTask = claimService.DeleteClaimAsync(id);
            var deleteAuditTask = auditService.AuditCoverAsync(id, Consts.HttpRequestTypeDelete);

            Task.WaitAll(deleteClaimTask, deleteAuditTask);

            return Task.FromResult<ActionResult>(Ok());
        }

        [HttpGet("{id}")]
        public async Task<ClaimModel> GetAsync(string id)
        {
            return new ClaimModel(await claimService.GetClaimAsync(id));
        }
    }
}
