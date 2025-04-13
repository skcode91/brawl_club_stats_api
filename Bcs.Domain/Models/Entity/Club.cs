namespace Bcs.Domain.Models.Entity;

public class Club : TimeStampedEntity
{
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Trophies { get; set; }
    public int RequiredTrophies { get; set; }
    public string Type { get; set; } = string.Empty;
    public int BadgeId { get; set; }
    
    public IList<Player> Players { get; set; } = new List<Player>();

    public IList<ClubSynchronizationHistory> ClubSynchronizationHistories { get; set; } = new List<ClubSynchronizationHistory>();

}