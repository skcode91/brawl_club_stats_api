namespace Bcs.Domain.Models.Entity;

public class Player
{
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public string Role { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public string? ClubTag { get; set; }
    public Club Club { get; set; } = null!;
    
    public int? LatestStatsId { get; set; }
    public PlayerStats LatestStats { get; set; } = null!;
    public IList<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
}