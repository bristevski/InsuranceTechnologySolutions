using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace Claims.Controllers;

/// <summary>
/// Claims controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClaimsController(IClaimService claimService, IAuditService auditService, IGuidProvider guidProvider, IClaimValidator validator) : ControllerBase
{
    /// <summary>
    /// Gets all claims
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return Ok((await claimService.GetClaimsAsync()).Select(x => new ClaimModel(x)));
    }

    /// <summary>
    /// Gets claim by Id
    /// </summary>
    /// <param name="id">Id of the claim</param>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        return Ok(new ClaimModel(await claimService.GetClaimAsync(id)));
    }

    /// <summary>
    /// Creates new claim and claimAudit type 'POST'
    /// </summary>
    /// <param name="claimModel">Model containing claim data</param>
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

    /// <summary>
    /// Deletes claim and creates new claimAudit type 'Delete'
    /// </summary>
    /// <param name="id">Id of the claim that needs to be deleted</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await claimService.DeleteClaimAsync(id);

        BackgroundJob.Enqueue(() => auditService.AuditClaimAsync(id, Consts.HttpRequestTypeDelete));

        return Ok();
    }    
}
