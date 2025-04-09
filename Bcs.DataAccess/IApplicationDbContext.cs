using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bcs.DataAccess;

public interface IApplicationDbContext
{
    DbSet<Club> Clubs { get; }
    DbSet<ClubStats> ClubStats { get; }
    DbSet<Player> Players { get; }
    DbSet<PlayerStats> PlayerStats { get; }
    DbSet<PlayerSynchronizationHistory> PlayerSynchronizationHistories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation);
}