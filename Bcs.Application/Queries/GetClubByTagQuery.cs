using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Queries;

public record GetClubByTagQuery(string Tag) : IRequest<ClubDto>;

internal class GetClubByTagQueryHandler : IRequestHandler<GetClubByTagQuery, ClubDto>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public GetClubByTagQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<ClubDto> Handle(GetClubByTagQuery request, CancellationToken cancellationToken)
    {
        var club = await _applicationDbContext.Clubs
            .Include(c => c.Players)
            .FirstOrDefaultAsync(c => c.Tag == request.Tag, cancellationToken)
                ?? throw new Exception($"Failed to retrieve club with tag: {request.Tag}");

        return ClubMapper.MapToClubDto(club);
    }
}
