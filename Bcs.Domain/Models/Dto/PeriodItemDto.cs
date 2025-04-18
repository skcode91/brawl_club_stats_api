namespace Bcs.Domain.Models.Dto;

public class PeriodItemDto
{
    public PlayerDetailsDto? PlayerOnStart { get; set; }
    public PlayerOfPeriodDto PlayerOnEnd { get; set; } = null!;
}