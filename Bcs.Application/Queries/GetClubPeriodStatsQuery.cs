using Bcs.Application.Extensions;
using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using Bcs.Domain.Models.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetClubPeriodStatsQuery(string? ClubTag, DateTime PeriodStart, DateTime PeriodEnd) : IRequest<ClubPeriodDto>;

internal class GetClubPeriodStatsQueryHandler : IRequestHandler<GetClubPeriodStatsQuery, ClubPeriodDto>
{
    private readonly IApplicationDbContext _context;

    public GetClubPeriodStatsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ClubPeriodDto> Handle(GetClubPeriodStatsQuery request, CancellationToken cancellationToken)
    {
        var startOfPeriod = request.PeriodStart.GetFirstHourOfDay();
        var endOfPeriod = request.PeriodEnd.GetFirstHourOfNextDay();
        
        var playerStatsAtStartOfPeriod = await _context.ClubSynchronizationHistories
            .AsNoTracking()
            .Include(x => x.Club)
            .Include(x => x.PlayerStats)
                .ThenInclude(x => x.Player)
            .WhereCreatedAtInHour(request.PeriodStart)
            .Where(x => string.IsNullOrEmpty(request.ClubTag) || x.ClubTag == request.ClubTag)
            .ToListAsync(cancellationToken);
        
        var playerStatsAtEndOfPeriod = await _context.ClubSynchronizationHistories
            .Include(x => x.Club)
            .Include(x => x.PlayerStats)
                .ThenInclude(x => x.Player)
            .WhereCreatedAtInHour(request.PeriodEnd)
            .Where(x => string.IsNullOrEmpty(request.ClubTag) || x.ClubTag == request.ClubTag)
            .ToListAsync(cancellationToken);

        if (!playerStatsAtEndOfPeriod.Any())
        {
            throw new Exception("No player stats found for the given date.");
        }

        var statsOnStart = playerStatsAtStartOfPeriod
            .SelectMany(x => x.PlayerStats);
        
        var statsOnEnd = playerStatsAtEndOfPeriod
            .SelectMany(x => x.PlayerStats)
            .ToList();
        
        var playerStatPairs = new List<(PlayerStats Start, PlayerStats End)>();

        foreach (var playerStat in statsOnEnd)
        {
            var statsAtStart = statsOnStart
                .FirstOrDefault(x => x.PlayerTag == playerStat.PlayerTag);

            if (statsAtStart is not null)
            {
                playerStatPairs.Add((statsAtStart, playerStat));
            }
        }

        playerStatPairs = playerStatPairs
            .OrderByDescending(x => x.End.Trophies - x.Start.Trophies)
            .ToList();

        var responsePlayers = new List<PeriodItemDto>();

        responsePlayers.AddRange(playerStatPairs.Select((x, index) => new PeriodItemDto()
        {
            PlayerOnStart = PlayerMapper.MapToPlayerDetailsDto(x.Start.Player),
            PlayerOnEnd = PlayerMapper.MapToPlayerOfPeriodDto(x.End.Player, index + 1, x.End.Trophies - x.Start.Trophies)
        }));

        return new ClubPeriodDto()
        {
            PeriodStart = startOfPeriod.ToString("d"),
            PeriodEnd = endOfPeriod.ToString("d"),
            Players = responsePlayers
        };
    }
}