using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Validators;
using Claims.Core.Claims.Entities;
using NSubstitute;

namespace Claims.Application.Tests.Validators;

public class ClaimValidatorTests
{
    public ClaimValidator _sut;
    public ICoverService _coverService;
    public string _coverId = "CoverId";
    public DateTime _startDate = new DateTime(2025,2,10);
    public DateTime _endDate = new DateTime(2025, 2, 20);
    public Cover _cover;

    public ClaimValidatorTests()
    {
        // Arrange
        _cover = new Cover()
        {
            Id = _coverId,
            StartDate = _startDate,
            EndDate = _endDate,
        };

        _coverService = Substitute.For<ICoverService>();
        _coverService.GetCoverAsync(_coverId).Returns(Task.FromResult(_cover));
        _sut = new ClaimValidator(_coverService);
    }

    [Fact]
    public async Task Should_ReturnNoErrorForValidModel()
    {
        // Arrange
        var claimModel = new ClaimModel()
        {
            CoverId = _coverId,
            DamageCost = 500,
            Created = _startDate.AddDays(2)
        };

        // Act
        var errors = await _sut.ValidateModel(claimModel);

        // Assert
        Assert.NotNull(errors);
        Assert.Empty(errors);
    }

    [Fact]
    public async Task Should_ReturnErrorForInvalidModel()
    {
        // Arrange
        var claimModel = new ClaimModel()
        {
            CoverId = _coverId,
            DamageCost = 1500,
            Created = _startDate.AddDays(-2)
        };

        // Act
        var errors = await _sut.ValidateModel(claimModel);

        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.Equal(ClaimErrorMessages.DamageCostTooHigh, errors[0]);
        Assert.Equal(ClaimErrorMessages.InvalidCreationDate, errors[1]);
    }
}
