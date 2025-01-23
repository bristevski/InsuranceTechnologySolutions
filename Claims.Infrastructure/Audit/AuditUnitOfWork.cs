using Claims.Core;
using Claims.Core.Audit.Entities;
using Claims.Core.Audit.Interfaces;

namespace Claims.Infrastructure.Audit;

public class AuditUnitOfWork : IAuditUnitOfWork
{
    private readonly AuditContext _context;

    private IGenericRepository<ClaimAudit> _claimAuditsRepository;
    private IGenericRepository<CoverAudit> _coverAuditsRepository;

    public AuditUnitOfWork(AuditContext context)
    {
        _context = context;
    }

    public IGenericRepository<ClaimAudit> ClaimAudits
    {
        get
        {
            return _claimAuditsRepository ??= new AuditGenericRepository<ClaimAudit>(_context);
        }
    }

    public IGenericRepository<CoverAudit> CoverAudits
    {
        get
        {
            return _coverAuditsRepository ??= new AuditGenericRepository<CoverAudit>(_context);
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
