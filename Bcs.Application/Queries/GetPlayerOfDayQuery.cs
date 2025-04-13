using Bcs.Application.Extensions;
using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetPlayerOfDayQuery(DateTime date, string? ClubTag) : IRequest<PlayerOfPeriodDto>;

internal class GetPlayerOfDayQueryHandler : IRequestHandler<GetPlayerOfDayQuery, PlayerOfPeriodDto>
{
    private readonly IApplicationDbContext _context;

    public GetPlayerOfDayQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerOfPeriodDto> Handle(GetPlayerOfDayQuery request, CancellationToken cancellationToken)
    {
        var startOfPeriod = request.date.GetFirstHourOfDay();
        var endOfPeriod = request.date.GetFirstHourOfNextDay();

        var playerStatsAtStartOfDay = await _context.ClubSynchronizationHistories
            .AsNoTracking()
            .Include(x => x.Club)
            .Include(x => x.PlayerStats)
            .ThenInclude(x => x.Player)
            .WhereCreatedAtInHour(startOfPeriod)
            .Where(x => string.IsNullOrEmpty(request.ClubTag) || x.ClubTag == request.ClubTag)
            .ToListAsync(cancellationToken);

        var playerStatsAtEndOfDay = await _context.ClubSynchronizationHistories
            .Include(x => x.Club)
            .Include(x => x.PlayerStats)
            .ThenInclude(x => x.Player)
            .WhereCreatedAtInHour(endOfPeriod)
            .Where(x => string.IsNullOrEmpty(request.ClubTag) || x.ClubTag == request.ClubTag)
            .ToListAsync(cancellationToken);

        if (!playerStatsAtStartOfDay.Any() && !playerStatsAtEndOfDay.Any())
        {
            throw new Exception("No player stats found for the given date.");
        }

        var allPlayerStats = playerStatsAtStartOfDay.SelectMany(x => x.PlayerStats)
            .Concat(playerStatsAtEndOfDay.SelectMany(x => x.PlayerStats))
            .ToList();

        var playersWithStatsPair = allPlayerStats
            .GroupBy(x => x.PlayerTag)
            .Where(g => g.Count() == 2)
            .Select(g => g.OrderBy(x => x.CreatedAt).ToList())
            .ToList();

        if (!playersWithStatsPair.Any())
        {
            throw new Exception("No player stats found for the given date.");
        }

        var playerOfDayStats = playersWithStatsPair
            .MaxBy(x => x.Last().Trophies - x.First().Trophies);

        if (playerOfDayStats == null)
        {
            throw new Exception("No player found for the given date.");
        }
        
        var result = playerOfDayStats.Last().Trophies - playerOfDayStats.First().Trophies;

        return new PlayerOfPeriodDto()
        {
            Tag = playerOfDayStats.Last().PlayerTag,
            Name = playerOfDayStats.Last().Player.Name,
            Trophies = playerOfDayStats.Last().Trophies,
            ClubTag = playerOfDayStats.Last().Player.ClubTag ?? string.Empty,
            ClubName = playerOfDayStats.Last().Player.Club.Name,
            Result = result,
            Rank = 1
        };
    }
}