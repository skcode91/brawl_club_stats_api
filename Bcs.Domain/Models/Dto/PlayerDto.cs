namespace Bcs.Domain.Models.Dto;

public class PlayerDto
{
    public required string Tag { get; set; }
    public required string Name { get; set; }
    public int Trophies { get; set; }
    public string Role { get; set; }
}