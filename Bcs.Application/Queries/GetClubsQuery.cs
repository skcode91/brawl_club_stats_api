using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetClubsQuery : IRequest<List<ClubBaseInfoDto>>;

internal class GetClubsQueryHandler : IRequestHandler<GetClubsQuery, List<ClubBaseInfoDto>>
{
    private readonly IApplicationDbContext _context;

    public GetClubsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClubBaseInfoDto>> Handle(GetClubsQuery request, CancellationToken cancellationToken) =>
        await _context.Clubs
            .AsNoTracking()
            .OrderByDescending(c => c.Trophies)
            .Select(x => new ClubBaseInfoDto
            {
                Tag = x.Tag,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken);
}