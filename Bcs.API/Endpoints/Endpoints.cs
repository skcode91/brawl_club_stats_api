using Bcs.Application.Commands;
using Bcs.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bcs.API.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/api/club/ranking", async ([FromQuery] string? tag, [FromServices] IMediator mediator) =>
        {
            var ranking = await mediator.Send(new GetClubRankingQuery(tag));
            return Results.Ok(ranking);
        });

        app.MapGet("/api/club/{tag}", async (string tag, [FromServices] IMediator mediator) =>
        {
            var club = await mediator.Send(new GetClubByTagQuery(tag));
            return Results.Ok(club);
        });

        app.MapPost("/api/club/synchronize/{tag}", async (string tag, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new SynchronizeClubCommand(tag));
            return Results.Ok(result);
        });
        
        app.MapGet("/api/ranking", async ([FromServices] IMediator mediator) =>
        {
            var ranking = await mediator.Send(new GetFullRankingQuery());
            return Results.Ok(ranking);
        });

        app.MapGet("/api/player-of-day", async ([FromServices] IMediator mediator, 
            [FromQuery] DateTime date, 
            [FromQuery] string? clubTag) =>
        {
            var playerOfDayQuery = new GetPlayerOfDayQuery(date, clubTag);

            var playerOfDay = await mediator.Send(playerOfDayQuery);
            return Results.Ok(playerOfDay);
        });
        
        app.MapGet("/api/clubs", async ([FromServices] IMediator mediator) =>
        {
            var clubs = await mediator.Send(new GetClubsQuery());
            return Results.Ok(clubs);
        });
        
        app.MapGet("/api/club-period-stats", async ([FromServices] IMediator mediator, 
            [FromQuery] string? clubTag, 
            [FromQuery] DateTime periodStart, 
            [FromQuery] DateTime periodEnd) =>
        {
            var clubPeriodStatsQuery = new GetClubPeriodStatsQuery(clubTag, periodStart, periodEnd);
            var clubPeriodStats = await mediator.Send(clubPeriodStatsQuery);
            return Results.Ok(clubPeriodStats);
        });
    }
}