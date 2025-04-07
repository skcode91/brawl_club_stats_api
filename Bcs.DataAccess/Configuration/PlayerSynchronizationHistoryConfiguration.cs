using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class PlayerSynchronizationHistoryConfiguration : IEntityTypeConfiguration<PlayerSynchronizationHistory>
{
    public void Configure(EntityTypeBuilder<PlayerSynchronizationHistory> builder)
    {
        builder.ToTable("PlayerSynchronizationHistory");
        builder.HasKey(psh => psh.Id);

        builder.Property(psh => psh.CreatedAt).IsRequired();

        builder.HasMany(psh => psh.PlayerStats)
            .WithOne(ps => ps.PlayerSynchronizationHistory)
            .HasForeignKey(ps => ps.PlayerSynchronizationHistoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}