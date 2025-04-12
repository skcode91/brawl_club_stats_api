using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetFullRankingQuery : IRequest<List<PlayerRankingDto>>;

internal class GetFullRankingQueryHandler : IRequestHandler<GetFullRankingQuery, List<PlayerRankingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFullRankingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<PlayerRankingDto>> Handle(GetFullRankingQuery request, CancellationToken cancellationToken)
    {
        var players = await _context.Players
            .Include(p => p.Club)
            .OrderByDescending(p => p.Trophies)
            .ToListAsync(cancellationToken);
        
        var playerRankings = players
            .Select((player, index) => PlayerMapper.MapToPlayerRankingDto(player, index + 1))
            .ToList();
        
        return playerRankings;
    }
}