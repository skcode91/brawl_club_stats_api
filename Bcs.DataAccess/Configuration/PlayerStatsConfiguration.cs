using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class PlayerStatsConfiguration : IEntityTypeConfiguration<PlayerStats>
{
    public void Configure(EntityTypeBuilder<PlayerStats> builder)
    {
        builder.ToTable("PlayerStats");
        builder.HasKey(ps => ps.Id);

        builder.HasOne(ps => ps.Player)
            .WithMany(p => p.PlayerStats)
            .HasForeignKey(ps => ps.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ps => ps.PlayerSynchronizationHistory)
            .WithMany(p => p.PlayerStats)
            .HasForeignKey(ps => ps.PlayerSynchronizationHistoryId)
            .OnDelete(DeleteBehavior.Restrict);   
    }
}