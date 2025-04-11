using Bcs.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bcs.DataAccess.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");
        builder.HasKey(p => p.Tag);
        builder.HasIndex(p => p.Tag)
            .IsUnique()
            .HasDatabaseName("IX_Players_Tag");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.PlayerStats)
            .WithOne(ps => ps.Player)
            .HasForeignKey(ps => ps.PlayerTag)
            .OnDelete(DeleteBehavior.Restrict);
    }
}