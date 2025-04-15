using Bcs.Domain.Models.Dto;
using MediatR;

namespace Bcs.Application.Queries;

public record GetFullRankingQuery : IRequest<List<PlayerRankingDto>>;

internal class GetFullRankingQueryHandler : IRequestHandler<GetFullRankingQuery, List<PlayerRankingDto>>
{
    private readonly IMediator _mediator;

    public GetFullRankingQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<PlayerRankingDto>> Handle(GetFullRankingQuery request, CancellationToken cancellationToken)
        => await _mediator.Send(new GetClubRankingQuery(null), cancellationToken);
}