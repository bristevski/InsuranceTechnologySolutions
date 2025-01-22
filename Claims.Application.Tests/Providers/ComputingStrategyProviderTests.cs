using Claims.Application.Providers;
using Claims.Core.Claims.Entities.Enums;

namespace Claims.Application.Tests.Providers
{
    public class ComputingStrategyProviderTests
    {
        private ComputingStrategyProvider _sut;
        private DateTime _startDate;

        public ComputingStrategyProviderTests()
        {
            // Arrange
            _startDate = new DateTime(2025, 2, 10);
            _sut = new ComputingStrategyProvider();
        }

        [Theory]
        [InlineData(10, 13750)]
        [InlineData(100, 132687.5)]
        [InlineData(200, 263862.5)]
        public void Should_ComputePremium_ForYacht(int days, decimal expectedPremium)
        {
            // Arrange
            var type = CoverType.Yacht;
            var endDate = _startDate.AddDays(days);

            // Act
            var premium = _sut.ComputePremium(_startDate, endDate, type);

            // Assert
            Assert.Equal(expectedPremium, premium);
        }

        [Theory]
        [InlineData(10, 16250)]
        [InlineData(100, 160225)]
        [InlineData(200, 319800)]
        public void Should_ComputePremium_ContainerShip(int days, decimal expectedPremium)
        {
            // Arrange
            var type = CoverType.ContainerShip;
            var endDate = _startDate.AddDays(days);

            // Act
            var premium = _sut.ComputePremium(_startDate, endDate, type);

            // Assert
            Assert.Equal(expectedPremium, premium);
        }

        [Theory]
        [InlineData(10, 18750)]
        [InlineData(100, 184875)]
        [InlineData(200, 369000)]
        public void Should_ComputePremium_Tanker(int days, decimal expectedPremium)
        {
            // Arrange
            var type = CoverType.Tanker;
            var endDate = _startDate.AddDays(days);

            // Act
            var premium = _sut.ComputePremium(_startDate, endDate, type);

            // Assert
            Assert.Equal(expectedPremium, premium);
        }

        [Theory]
        [InlineData(10, 16250)]
        [InlineData(100, 160225)]
        [InlineData(200, 319800)]
        public void Should_ComputePremium_BulkCarrier(int days, decimal expectedPremium)
        {
            // Arrange
            var type = CoverType.BulkCarrier;
            var endDate = _startDate.AddDays(days);

            // Act
            var premium = _sut.ComputePremium(_startDate, endDate, type);

            // Assert
            Assert.Equal(expectedPremium, premium);
        }

        [Theory]
        [InlineData(10, 15000)]
        [InlineData(100, 147900)]
        [InlineData(200, 295200)]
        public void Should_ComputePremium_PassengerShip(int days, decimal expectedPremium)
        {
            // Arrange
            var type = CoverType.PassengerShip;
            var endDate = _startDate.AddDays(days);

            // Act
            var premium = _sut.ComputePremium(_startDate, endDate, type);

            // Assert
            Assert.Equal(expectedPremium, premium);
        }
    }
}
