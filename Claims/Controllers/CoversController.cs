using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Models.Enums;
using Claims.Application;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IEnumerable<CoverModel>> GetAsync()
    {
        return (await coverService.GetCoversAsync()).Select(x => new CoverModel(x));
    }

    [HttpGet("{id}")]
    public async Task<CoverModel> GetAsync(string id)
    {
        return new CoverModel(await coverService.GetCoverAsync(id));
    }

    [HttpPost]
    public Task<ActionResult> CreateAsync(CoverModel coverModel)
    {
        var errors = validator.ValidateModel(coverModel);
        if (errors.Any())
            return Task.FromResult<ActionResult>(BadRequest(errors));

        coverModel.Id = guidProvider.NewStringGuid();

        var addCoverTask = coverService.AddCoverAsync(coverModel.ToDomainModel());
        var addAuditTask = auditService.AuditCoverAsync(coverModel.Id, Consts.HttpRequestTypePost);

        Task.WaitAll(addCoverTask, addAuditTask);
        return Task.FromResult<ActionResult>(Ok(coverModel));
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> DeleteAsync(string id)
    {
        var deleteCoverTask = coverService.DeleteCoverAsync(id);
        var deleteAuditTask = auditService.AuditCoverAsync(id, Consts.HttpRequestTypeDelete);

        Task.WaitAll(deleteCoverTask, deleteAuditTask);
        return Task.FromResult<ActionResult>(Ok());
    }
}
