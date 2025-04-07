namespace Bcs.Integration.BrawlStars.Models;

public class BrawlStarsMember
{
    public BrawlStarsPlayerIcon Icon { get; set; } = null!;
    public string Tag { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Trophies { get; set; }
    public string Role { get; set; } = string.Empty;
    public string NameColor { get; set; } = string.Empty;
}