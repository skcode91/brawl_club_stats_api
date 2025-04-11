using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class ClubSynchronizationHistoryConfiguration : IEntityTypeConfiguration<ClubSynchronizationHistory>
{
    public void Configure(EntityTypeBuilder<ClubSynchronizationHistory> builder)
    {
        builder.ToTable("ClubSynchronizationHistory");
        builder.HasKey(psh => psh.Id);

        builder.Property(psh => psh.CreatedAt).IsRequired();

        builder.HasMany(psh => psh.PlayerStats)
            .WithOne(ps => ps.PlayerSynchronizationHistory)
            .HasForeignKey(ps => ps.ClubSynchronizationHistoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(psh => psh.Club)
            .WithMany(c => c.ClubSynchronizationHistories)
            .HasForeignKey(psh => psh.ClubTag)
            .OnDelete(DeleteBehavior.Cascade);
    }
}