namespace Bcs.Domain.Models.Entity;

public class PlayerStats
{
    public int Id { get; set; }
    public int Trophies { get; set; }


    public required string PlayerTag { get; set; }
    public Player Player { get; set; } = null!;
    
    public int ClubSynchronizationHistoryId { get; set; }
    public ClubSynchronizationHistory PlayerSynchronizationHistory { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
}