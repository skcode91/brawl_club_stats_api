namespace Bcs.Domain.Models.Dto;

public class PlayerOfPeriodDto : PlayerRankingDto
{
    public int Result { get; set; }
    public required string PeriodStart { get; set; }
    public required string PeriodEnd { get; set; }
}