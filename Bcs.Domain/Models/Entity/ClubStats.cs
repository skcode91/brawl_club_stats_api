namespace Bcs.Domain.Models.Entity;

public class ClubStats
{
    public int Id { get; set; }
    
    public required string ClubTag { get; set; }
    public Club Club { get; set; } = null!;
    
    public int Trophies { get; set; }

    public DateTime CreatedAt { get; set; }
}