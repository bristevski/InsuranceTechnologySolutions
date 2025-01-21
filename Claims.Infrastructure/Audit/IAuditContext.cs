using Core.Audit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Claims.Infrastructure.Audit
{
    public interface IAuditContext
    {
        DbSet<ClaimAudit> ClaimAudits { get; }
        DbSet<CoverAudit> CoverAudits { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
