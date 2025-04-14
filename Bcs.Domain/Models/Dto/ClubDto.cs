namespace Bcs.Domain.Models.Dto;

public class ClubDto : ClubBaseInfoDto
{
    public string Description { get; set; } = string.Empty;
    public int Trophies { get; set; }
    public int RequiredTrophies { get; set; }
    public string Type { get; set; } = string.Empty;
    public List<PlayerDto> Members { get; set; } = new List<PlayerDto>();
    
}