using Claims.Application.Interfaces;
using Claims.Infrastructure.Audit;
using Claims.Core.Audit.Entities;

namespace Claims.Application.Services;

public class AuditService(IAuditUnitOfWork unitOfWork, TimeProvider timeProvider) : IAuditService
{
    public async Task AuditClaimAsync(string id, string httpRequestType)
    {
        var claimAudit = new ClaimAudit()
        {
            Created = timeProvider.GetUtcNow().DateTime,
            HttpRequestType = httpRequestType,
            ClaimId = id
        };

        await unitOfWork.ClaimAudits.AddAsync(claimAudit);
        await unitOfWork.SaveAsync();
    }

    public async Task AuditCoverAsync(string id, string httpRequestType)
    {
        var coverAudit = new CoverAudit()
        {
            Created = timeProvider.GetUtcNow().DateTime,
            HttpRequestType = httpRequestType,
            CoverId = id
        };

        await unitOfWork.CoverAudits.AddAsync(coverAudit);
        await unitOfWork.SaveAsync();
    }
}
