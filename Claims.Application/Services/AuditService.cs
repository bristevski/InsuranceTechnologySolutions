using Claims.Application.Interfaces;
using Claims.Infrastructure;
using Claims.Infrastructure.Audit;
using Core.Audit.Entities;

namespace Claims.Application.Services
{
    public class AuditService(IAuditUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider) : IAuditService
    {
        public async Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = dateTimeProvider.DateTimeNow(),
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
                Created = dateTimeProvider.DateTimeNow(),
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            await unitOfWork.CoverAudits.AddAsync(coverAudit);
            await unitOfWork.SaveAsync();
        }
    }
}
