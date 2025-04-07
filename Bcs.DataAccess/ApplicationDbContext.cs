using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Bcs.DataAccess;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<ClubStats> ClubStats => Set<ClubStats>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerStats> PlayerStats => Set<PlayerStats>();
    public DbSet<PlayerSynchronizationHistory> PlayerSynchronizationHistories => Set<PlayerSynchronizationHistory>();
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
         
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); 
    }
}