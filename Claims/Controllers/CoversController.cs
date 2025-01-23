using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Models.Enums;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace Claims.Controllers;

/// <summary>
/// Covers controller
/// </summary>
[ApiController]
[Route("[controller]")]
public class CoversController(ICoverService coverService, IAuditService auditService, IGuidProvider guidProvider, ICoverValidator validator) : ControllerBase
{   
    /// <summary>
    /// Gets all coverages
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return Ok((await coverService.GetCoversAsync()).Select(x => new CoverModel(x)));
    }

    /// <summary>
    /// Gets cover by Id
    /// </summary>
    /// <param name="id">Id of the cover</param>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        return Ok(new CoverModel(await coverService.GetCoverAsync(id)));
    }

    /// <summary>
    /// Creates new cover and coverAudit type 'POST'
    /// </summary>
    /// <param name="coverModel">Model containing cover data</param>
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CoverModel coverModel)
    {
        var errors = validator.ValidateModel(coverModel);
        if (errors.Any())
            return BadRequest(errors);

        coverModel.Id = guidProvider.NewStringGuid();

        await coverService.AddCoverAsync(coverModel.ToDomainModel());

        BackgroundJob.Enqueue(() => auditService.AuditCoverAsync(coverModel.Id, Consts.HttpRequestTypePost));

        return Ok(coverModel);
    }

    /// <summary>
    /// Computes premium for given period and cover type
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <param name="coverType">Type of cover</param>
    [HttpPost("compute")]
    public ActionResult ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverModelType coverType)
    {
        var coverModel = new CoverModel()
        {
            StartDate = startDate,
            EndDate = endDate,
            Type = coverType
        };
        return Ok(coverService.ComputePremium(coverModel.ToDomainModel()));
    }

    /// <summary>
    /// Deletes cover and creates new coverAudit type 'Delete'
    /// </summary>
    /// <param name="id">Id of the cover that needs to be deleted</param>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await coverService.DeleteCoverAsync(id);

        BackgroundJob.Enqueue(() => auditService.AuditCoverAsync(id, Consts.HttpRequestTypeDelete));

        return Ok();
    }
}
