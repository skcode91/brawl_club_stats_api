namespace Bcs.Domain.Models.Entity;

public class PlayerStats
{
    public int Id { get; set; }
    
    public required string PlayerTag { get; set; }
    public Player Player { get; set; } = null!;
    
    public int PlayerSynchronizationHistoryId { get; set; }
    public PlayerSynchronizationHistory PlayerSynchronizationHistory { get; set; } = null!;
    
    public int Trophies { get; set; }
}