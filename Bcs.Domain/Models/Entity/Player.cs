namespace Bcs.Domain.Models.Entity;

public class Player
{
    public int Id { get; set; }
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public string Role { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public int? ClubId { get; set; }
    public Club Club { get; set; } = null!;
    
    public int? LatestStatsId { get; set; }
    public PlayerStats LatestStats { get; set; } = null!;
    public IList<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>();
}