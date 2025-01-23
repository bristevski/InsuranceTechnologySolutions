using Claims.Core.Claims.Entities;

namespace Claims.Core.Claims.Interfaces;

public interface IClaimsUnitOfWork : IDisposable
{
    IGenericRepository<Claim> Claims { get; }
    IGenericRepository<Cover> Covers { get; }
    Task<int> SaveAsync();
}
