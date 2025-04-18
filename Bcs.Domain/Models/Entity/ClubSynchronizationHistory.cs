namespace Bcs.Domain.Models.Entity;

public class ClubSynchronizationHistory : TimeStampedEntity
{
    public int Id { get; set; }
    public int Trophies { get; set; }

    public string ClubTag { get; set; } = null!;
    public Club Club { get; set; } = null!;

    public IList<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
}