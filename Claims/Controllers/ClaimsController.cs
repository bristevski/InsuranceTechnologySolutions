using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;


namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController(IClaimService claimService, IAuditService auditService, IGuidProvider guidProvider, IClaimValidator validator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return Ok((await claimService.GetClaimsAsync()).Select(x => new ClaimModel(x)));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(ClaimModel claimModel)
    {
        var errors = await validator.ValidateModel(claimModel);
        if(errors.Any())
            return BadRequest(errors);

        claimModel.Id = guidProvider.NewStringGuid();

        var addClaimTask = claimService.AddClaimAsync(claimModel.ToDomainModel());
        var addAuditTask = auditService.AuditClaimAsync(claimModel.Id, Consts.HttpRequestTypePost);

        await Task.WhenAll(addClaimTask, addAuditTask);

        return Ok(claimModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var deleteClaimTask = claimService.DeleteClaimAsync(id);
        var deleteAuditTask = auditService.AuditClaimAsync(id, Consts.HttpRequestTypeDelete);

        await Task.WhenAll(deleteClaimTask, deleteAuditTask);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        return Ok(new ClaimModel(await claimService.GetClaimAsync(id)));
    }
}
