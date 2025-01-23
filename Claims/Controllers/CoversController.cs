using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Models.Enums;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;
using Hangfire;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ICoverService coverService, IAuditService auditService, IGuidProvider guidProvider, ICoverValidator validator) : ControllerBase
{
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

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return Ok((await coverService.GetCoversAsync()).Select(x => new CoverModel(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(string id)
    {
        return Ok(new CoverModel(await coverService.GetCoverAsync(id)));
    }

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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await coverService.DeleteCoverAsync(id);

        BackgroundJob.Enqueue(() => auditService.AuditCoverAsync(id, Consts.HttpRequestTypeDelete));

        return Ok();
    }
}
