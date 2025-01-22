using Core.Claims.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure.Claims
{
    public interface IClaimsContext
    {
        DbSet<Claim> Claims { get; }
        DbSet<Cover> Covers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class ClaimsContext : DbContext, IClaimsContext
    {
        public virtual DbSet<Claim> Claims { get; init; }
        public virtual DbSet<Cover> Covers { get; init; }

        public ClaimsContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Claim>().ToCollection("claims");
            modelBuilder.Entity<Cover>().ToCollection("covers");
        }
    }
}
