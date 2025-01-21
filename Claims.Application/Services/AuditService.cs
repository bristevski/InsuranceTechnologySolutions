using Claims.Application.Interfaces;
using Claims.Infrastructure.Audit;
using Core.Audit.Entities;

namespace Claims.Application.Services
{
    public class AuditService(IAuditContext dbContext, IDateTimeProvider dateTimeProvider) : IAuditService
    {
        public async Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = dateTimeProvider.DateTimeNow,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            dbContext.ClaimAudits.Add(claimAudit);
            await dbContext.SaveChangesAsync();
        }

        public async Task AuditCoverAsync(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = dateTimeProvider.DateTimeNow,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            dbContext.CoverAudits.Add(coverAudit);
            await dbContext.SaveChangesAsync();
        }
    }
}
