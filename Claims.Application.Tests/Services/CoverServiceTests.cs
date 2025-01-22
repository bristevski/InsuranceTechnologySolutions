using Claims.Application.Interfaces;
using Claims.Application.Services;
using Claims.Core.Claims.Entities.Enums;
using Claims.Infrastructure;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using NSubstitute;

namespace Claims.Application.Tests.Services
{
    public class CoverServiceTests
    {
        private CoverService _sut;
        private IComputingStrategyProvider _computingStrategyProvider;
        private IClaimsUnitOfWork _unitOfWork;
        private string _coverId = "Cover 1";
        private DateTime _startDate = new DateTime(2025, 2, 22);
        private DateTime _endDate = new DateTime(2025, 2, 25);
        private CoverType _type = CoverType.Yacht;

        public CoverServiceTests()
        {
            // Arrange
            var mockCovers = new List<Cover>
            {
                new Cover
                {
                    Id = _coverId,
                }
            };

            _unitOfWork = Substitute.For<IClaimsUnitOfWork>();
            var coversRepository = Substitute.For<IGenericRepository<Cover>>();

            coversRepository.GetAllAsync().Returns(Task.FromResult(mockCovers.AsEnumerable()));
            coversRepository.GetByIdAsync(_coverId).Returns(Task.FromResult(mockCovers.First(x => x.Id == _coverId)));

            _unitOfWork.Covers.Returns(coversRepository);

            _computingStrategyProvider = Substitute.For<IComputingStrategyProvider>();
            _computingStrategyProvider.ComputePremium(_startDate, _endDate, _type).Returns(1250);

            _sut = new CoverService(_unitOfWork, _computingStrategyProvider);
        }


        [Fact]
        public async Task Should_AddCoverAsync()
        {
            // Assert
            var cover = new Cover()
            {
                Id = "Cover 2",
                StartDate = _startDate,
                EndDate = _endDate,
                Type = _type
            };

            // Act
            var result = await _sut.AddCoverAsync(cover);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cover.Id, result.Id);
            Assert.Equal(1250, result.Premium);
            _computingStrategyProvider.Received(1).ComputePremium(_startDate, _endDate, _type);
            await _unitOfWork.Received(1).Covers.AddAsync(cover);
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task Should_GetAllAsync()
        {
            // Act
            var result = await _sut.GetCoversAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            await _unitOfWork.Received(1).Covers.GetAllAsync();
        }

        [Fact]
        public async Task Should_GetCoverAsync_WhenCoverExists()
        {
            // Act        
            var result = await _sut.GetCoverAsync(_coverId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, _coverId);
            await _unitOfWork.Received(1).Covers.GetByIdAsync(_coverId);
        }

        [Fact]
        public async Task Should_GetCoverAsync_WhenCoverDoesNotExists()
        {
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _sut.GetCoverAsync(""));
        }

        [Fact]
        public async Task Should_DeleteCoverAsync_WhenCoverExists()
        {
            // Act
            await _sut.DeleteCoverAsync(_coverId);

            // Assert
            await _unitOfWork.Received(2).Covers.GetByIdAsync(_coverId);
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task ShouldThrow_DeleteCoverAsync_WhenCoverDoesNotExists()
        {
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _sut.DeleteCoverAsync(""));
        }

        [Fact]
        public void Should_ComputePremium()
        {
            // Assert
            var cover = new Cover()
            {
                Id = "Cover 3",
                StartDate = _startDate,
                EndDate = _endDate,
                Type = _type
            };
            var premium = _sut.ComputePremium(cover);
            Assert.Equal(1250, premium);

        }
    }
}

