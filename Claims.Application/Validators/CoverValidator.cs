using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Providers;

namespace Claims.Application.Validators
{
    public class CoverValidator(IDateTimeProvider dateTimeProvider) : ICoverValidator
    {
        public List<string> ValidateModel(CoverModel coverModel)
        {
            var errors = new List<string>();

            var currentDateTime = dateTimeProvider.DateTimeNow();
            if (currentDateTime > coverModel.StartDate)
                errors.Add(CoverErrorMessages.StartDateInPast);

            if (coverModel.StartDate.AddYears(1) < coverModel.EndDate)
                errors.Add(CoverErrorMessages.PeriodGreaterThanOneYear);

            return errors;
        }
    }
}
