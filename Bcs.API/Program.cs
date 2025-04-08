using System.Reflection;
using Bcs.Application.Commands;
using Bcs.Application.Configuration;
using Bcs.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

DomainModule.SetupDomain(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.Run();
