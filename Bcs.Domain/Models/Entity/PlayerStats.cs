namespace Bcs.Domain.Models.Entity;

public class PlayerStats : TimeStampedEntity
{
    public int Id { get; set; }
    public int Trophies { get; set; }

    public required string PlayerTag { get; set; }
    public Player Player { get; set; } = null!;
    
    public int ClubSynchronizationHistoryId { get; set; }
    public ClubSynchronizationHistory PlayerSynchronizationHistory { get; set; } = null!;
}