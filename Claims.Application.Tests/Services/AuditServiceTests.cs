﻿using Claims.Application.Services;
using Claims.Core.Audit.Entities;
using NSubstitute;
using Claims.Core.Audit.Interfaces;

namespace Claims.Application.Tests.Services;

public class AuditServiceTests
{
    private AuditService _sut;
    private IAuditUnitOfWork _unitOfWork;
    private TimeProvider _timeProvider;
    private DateTime _creationDate;

    public AuditServiceTests()
    {
        // Arrange
        _unitOfWork = Substitute.For<IAuditUnitOfWork>();

        _creationDate = new DateTime(2025, 01, 22);
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.GetUtcNow().Returns(_creationDate);

        _sut = new AuditService(_unitOfWork, _timeProvider);
    }

    [Theory]
    [InlineData("Id1", "POST")]
    [InlineData("Id2", "DELETE")]
    public async Task Should_AuditClaimAsync(string id, string httpType)
    {
        // Act
        await _sut.AuditClaimAsync(id, httpType);

        // Assert
        await _unitOfWork.Received(1).ClaimAudits.AddAsync(Arg.Is<ClaimAudit>(ca =>
            ca.ClaimId == id &&
            ca.HttpRequestType == httpType &&
            ca.Created == _creationDate));
        await _unitOfWork.Received(1).SaveAsync();
    }

    [Theory]
    [InlineData("Id1", "POST")]
    [InlineData("Id2", "DELETE")]
    public async Task Should_AuditCoverAsync(string id, string httpType)
    {
        // Act
        await _sut.AuditCoverAsync(id, httpType);

        // Assert
        await _unitOfWork.Received(1).CoverAudits.AddAsync(Arg.Is<CoverAudit>(ca =>
            ca.CoverId == id &&
            ca.HttpRequestType == httpType &&
            ca.Created == _creationDate));
        await _unitOfWork.Received(1).SaveAsync();
    }
}
