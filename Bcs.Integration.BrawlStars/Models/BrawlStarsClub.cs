namespace Bcs.Integration.BrawlStars.Models;

public class BrawlStarsClub
{
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Trophies { get; set; }
    public int RequiredTrophies { get; set; }
    public List<BrawlStarsMember> Members { get; set; } = new();
    public string Type { get; set; } = string.Empty;
    public int BadgeId { get; set; }
}