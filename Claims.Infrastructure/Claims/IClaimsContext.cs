using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Claims
{
    public interface IClaimsContext
    {
        DbSet<Claim> Claims { get; }
        DbSet<Cover> Covers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
