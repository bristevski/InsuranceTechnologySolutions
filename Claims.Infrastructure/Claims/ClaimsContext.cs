using Claims.Core.Claims.Entities;
using Claims.Core.Claims.Interfaces;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure.Claims;

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
