using Claims.Application.Interfaces;
using Claims.Core.Audit.Interfaces;
using Claims.Infrastructure.Audit;
using Core.Audit.Entities;

namespace Claims.Application.Services
{
    public class AuditService(IAuditContext dbContext, IDateTimeProvider dateTimeProvider) : IAuditService
    {
        public IAuditContext _dbContext { get; } = dbContext;
        public IDateTimeProvider _dateTimeProvider { get; } = dateTimeProvider;

        public void AuditClaim(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = _dateTimeProvider.DateTimeNow,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _dbContext.ClaimAudits.Add(claimAudit);
            Task.Run(() => _dbContext.SaveChangesAsync());
        }

        public void AuditCover(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = _dateTimeProvider.DateTimeNow,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _dbContext.CoverAudits.Add(coverAudit);
            Task.Run(() => _dbContext.SaveChangesAsync());
        }
    }
}
