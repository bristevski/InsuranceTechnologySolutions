using Claims.Application.Services;
using Claims.Infrastructure;
using Claims.Infrastructure.Claims;
using Core.Claims.Entities;
using NSubstitute;

namespace Claims.Application.Tests
{
    public class ClaimServiceTests
    {
        private ClaimService _sut;
        private IClaimsUnitOfWork _unitOfWork;
        private string _claimId = "Claim 1";

        public ClaimServiceTests()
        {
            // Arrange
            var mockClaims = new List<Claim>
            {
                new Claim { Id = _claimId }
            };

            _unitOfWork = Substitute.For<IClaimsUnitOfWork>();
            var claimsRepository = Substitute.For<IGenericRepository<Claim>>();

            claimsRepository.GetAllAsync().Returns(Task.FromResult(mockClaims.AsEnumerable()));
            claimsRepository.GetByIdAsync(_claimId).Returns(Task.FromResult(mockClaims.First(x => x.Id == _claimId)));

            _unitOfWork.Claims.Returns(claimsRepository);
            _sut = new ClaimService(_unitOfWork);
        }


        [Fact]
        public async Task Should_AddClaimsAsync()
        {
            // Assert
            var claim = new Claim() { Id = "Claim 2" };

            // Act
            var result = await _sut.AddClaimAsync(claim);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(claim.Id, result.Id);
            await _unitOfWork.Received(1).Claims.AddAsync(claim);
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task Should_GetAllAsync()
        {
            // Act
            var result = await _sut.GetClaimsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            await _unitOfWork.Received(1).Claims.GetAllAsync();
        }

        [Fact]
        public async Task Should_GetClaimAsync_WhenClaimExists()
        {
            // Act        
            var result = await _sut.GetClaimAsync(_claimId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, _claimId);
            await _unitOfWork.Received(1).Claims.GetByIdAsync(_claimId);
        }

        [Fact]
        public async Task ShouldThrow_GetClaimAsync_WhenClaimDoesNotExists()
        {
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _sut.GetClaimAsync(""));
        }

        [Fact]
        public async Task Should_DeleteClaimAsync_WhenClaimExists()
        {
            // Act
            await _sut.DeleteClaimAsync(_claimId);

            // Assert
            await _unitOfWork.Received(2).Claims.GetByIdAsync(_claimId);
            await _unitOfWork.Received(1).SaveAsync();
        }

        [Fact]
        public async Task ShouldThrow_DeleteClaimAsync_WhenClaimDoesNotExists()
        {
            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _sut.DeleteClaimAsync(""));
        }
    }
}

