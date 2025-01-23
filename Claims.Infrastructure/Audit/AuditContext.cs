using Claims.Core.Audit.Entities;
using Claims.Core.Audit.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Audit;

public class AuditContext : DbContext, IAuditContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }
    public DbSet<ClaimAudit> ClaimAudits { get; set; }
    public DbSet<CoverAudit> CoverAudits { get; set; }
}
