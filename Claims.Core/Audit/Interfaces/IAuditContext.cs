using Claims.Core.Audit.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Core.Audit.Interfaces;

public interface IAuditContext
{
    DbSet<ClaimAudit> ClaimAudits { get; }
    DbSet<CoverAudit> CoverAudits { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
