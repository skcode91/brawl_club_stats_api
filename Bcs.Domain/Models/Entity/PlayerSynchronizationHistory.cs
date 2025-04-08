namespace Bcs.Domain.Models.Entity;

public class PlayerSynchronizationHistory
{
    public int Id { get; set; }
    public IList<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
    public DateTime CreatedAt { get; set; }
}