using Bcs.Application.Mapping;
using Bcs.Domain.Models.Dto;
using Bcs.Integration.BrawlStars;
using MediatR;

namespace Bcs.Application.Queries;

public record GetClubByTagQuery(string Tag) : IRequest<ClubDto>;

internal class GetClubByTagQueryHandler : IRequestHandler<GetClubByTagQuery, ClubDto>
{
    private readonly IBrawlStarsService _brawlStarsService;

    public GetClubByTagQueryHandler(IBrawlStarsService brawlStarsService)
    {
        _brawlStarsService = brawlStarsService;
    }

    public async Task<ClubDto> Handle(GetClubByTagQuery request, CancellationToken cancellationToken)
    {
        var club = await _brawlStarsService.GetClubAsync(request.Tag, cancellationToken);
        
        if (club == null)
        {
            throw new Exception($"Failed to retrieve club with tag: {request.Tag}");
        }

        return ClubMapper.MapToClubDto(club);
    }
}
