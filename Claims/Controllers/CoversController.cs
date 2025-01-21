using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ICoverService coverService, ICoverValidator validator) : ControllerBase
{
    [HttpPost("compute")]
    public ActionResult ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverModelType coverType)
    {
        return Ok(coverService.ComputePremium(startDate, endDate, coverType));
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
    public async Task<ActionResult> CreateAsync(CoverModel coverModel)
    {
        var errors = validator.ValidateModel(coverModel);
        if (errors.Any())
            return BadRequest(errors);

        var cover = await coverService.AddCoverAsync(coverModel.ToDomainModel());
        coverModel.Id = cover.Id;
        return Ok(coverModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await coverService.DeleteCoverAsync(id);
        return Ok();
    }
}
