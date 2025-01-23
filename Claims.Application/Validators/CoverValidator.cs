using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Application.Validators;

public class CoverValidator(TimeProvider timeProvider) : ICoverValidator
{
    public List<string> ValidateModel(CoverModel coverModel)
    {
        var errors = new List<string>();

        var currentDateTime = timeProvider.GetUtcNow().DateTime;
        if (currentDateTime > coverModel.StartDate)
            errors.Add(CoverErrorMessages.StartDateInPast);

        if (coverModel.StartDate.AddYears(1) < coverModel.EndDate)
            errors.Add(CoverErrorMessages.PeriodGreaterThanOneYear);

        return errors;
    }
}
