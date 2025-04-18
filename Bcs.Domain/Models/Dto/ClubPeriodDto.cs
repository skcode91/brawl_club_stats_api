namespace Bcs.Domain.Models.Dto;

public class ClubPeriodDto
{
    public required string PeriodStart { get; set; }
    public required string PeriodEnd { get; set; }
    public List<PeriodItemDto> Players { get; set; } = [];
}