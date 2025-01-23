using Claims.Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Core.Claims.Interfaces;

public interface IClaimsContext
{
    DbSet<Claim> Claims { get; }
    DbSet<Cover> Covers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
