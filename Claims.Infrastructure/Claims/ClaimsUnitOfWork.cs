using Claims.Core.Claims.Entities;

namespace Claims.Infrastructure.Claims;

public interface IClaimsUnitOfWork : IDisposable
{
    IGenericRepository<Claim> Claims { get; }
    IGenericRepository<Cover> Covers { get; }
    Task<int> SaveAsync();
}

public class ClaimsUnitOfWork : IClaimsUnitOfWork
{
    private readonly ClaimsContext _context;

    private IGenericRepository<Claim> _claimRepository;
    private IGenericRepository<Cover> _coverRepository;

    public ClaimsUnitOfWork(ClaimsContext context)
    {
        _context = context;
    }

    public IGenericRepository<Claim> Claims
    {
        get
        {
            return _claimRepository ??= new ClaimsGenericRepository<Claim>(_context);
        }
    }

    public IGenericRepository<Cover> Covers
    {
        get
        {
            return _coverRepository ??= new ClaimsGenericRepository<Cover>(_context);
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
