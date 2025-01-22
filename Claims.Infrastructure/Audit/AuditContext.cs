using Core.Audit.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Audit
{
    public interface IAuditContext
    {
        DbSet<ClaimAudit> ClaimAudits { get; }
        DbSet<CoverAudit> CoverAudits { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class AuditContext : DbContext, IAuditContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }
        public DbSet<ClaimAudit> ClaimAudits { get; set; }
        public DbSet<CoverAudit> CoverAudits { get; set; }
    }
}
