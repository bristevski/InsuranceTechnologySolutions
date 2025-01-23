using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Validators;
using NSubstitute;

namespace Claims.Application.Tests.Validators;

public class CoverValidatorTests
{
    private CoverValidator _sut;
    private TimeProvider _timeProvider;
    private DateTime _currentDateTime = new DateTime(2025, 2, 10);

    public CoverValidatorTests()
    {
        // Arrange
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(_currentDateTime);
        _sut = new CoverValidator(_timeProvider);
    }

    [Fact]
    public void Should_ReturnNoErrorForValidModel()
    {
        // Arrange
        var coverMode = new CoverModel()
        {
            StartDate = _currentDateTime.AddDays(5),
            EndDate = _currentDateTime.AddDays(35)
        };

        // Act
        var errors = _sut.ValidateModel(coverMode);

        // Assert
        Assert.NotNull(errors);
        Assert.Empty(errors);
    }

    [Fact]
    public void Should_ReturnErrorForInvalidModel()
    {
        // Arrange
        var coverModel = new CoverModel()
        {
            StartDate = _currentDateTime.AddDays(-5),
            EndDate = _currentDateTime.AddYears(2)
        };

        // Act
        var errors = _sut.ValidateModel(coverModel);

        // Assert
        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.Equal(CoverErrorMessages.StartDateInPast, errors[0]);
        Assert.Equal(CoverErrorMessages.PeriodGreaterThanOneYear, errors[1]);
    }
}
