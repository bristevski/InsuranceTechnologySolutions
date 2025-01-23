using Claims.Core.Audit.Entities;

namespace Claims.Core.Audit.Interfaces;

public interface IAuditUnitOfWork : IDisposable
{
    IGenericRepository<ClaimAudit> ClaimAudits { get; }
    IGenericRepository<CoverAudit> CoverAudits { get; }
    Task<int> SaveAsync();
}
