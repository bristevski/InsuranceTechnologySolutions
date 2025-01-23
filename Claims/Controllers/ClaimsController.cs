using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;
using Hangfire;


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

        await claimService.AddClaimAsync(claimModel.ToDomainModel());

        BackgroundJob.Enqueue(() => auditService.AuditClaimAsync(claimModel.Id, Consts.HttpRequestTypePost));

        return Ok(claimModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await claimService.DeleteClaimAsync(id);

        BackgroundJob.Enqueue(() => auditService.AuditClaimAsync(id, Consts.HttpRequestTypeDelete));

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        return Ok(new ClaimModel(await claimService.GetClaimAsync(id)));
    }
}
