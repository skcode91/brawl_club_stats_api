namespace Bcs.Domain.Models.Entity;

public class Club
{
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int RequiredTrophies { get; set; }
    public string Type { get; set; } = string.Empty;
    public int BadgeId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int? LatestStatsId { get; set; }
    public ClubStats LatestStats { get; set; } = null!;
    
    public IList<Player> Players { get; set; } = new List<Player>();
    public IList<ClubStats> ClubStats { get; set; } = new List<ClubStats>();
}