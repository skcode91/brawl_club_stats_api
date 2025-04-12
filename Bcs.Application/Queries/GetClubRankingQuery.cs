using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetClubRankingQuery(string ClubTag) : IRequest<List<PlayerRankingDto>>;

internal class GetClubRankingQueryHandler : IRequestHandler<GetClubRankingQuery, List<PlayerRankingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetClubRankingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PlayerRankingDto>> Handle(GetClubRankingQuery request, CancellationToken cancellationToken)
    {
        var players = await _context.Players
            .Include(p => p.Club)
            .Where(p => p.Club.Tag == request.ClubTag)
            .OrderByDescending(p => p.Trophies)
            .ToListAsync(cancellationToken);
        
        var playerRankings = players
            .Select((player, index) => PlayerMapper.MapToPlayerRankingDto(player, index + 1))
            .ToList();
        
        return playerRankings;
    }
}