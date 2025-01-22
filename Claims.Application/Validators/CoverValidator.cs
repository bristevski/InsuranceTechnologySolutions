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
                errors.Add("Cover start date cannot be in the past");

            if (coverModel.StartDate.AddYears(1) < coverModel.EndDate)
                errors.Add("Cover period cannot be greater that 1 year");

            return errors;
        }
    }
}
