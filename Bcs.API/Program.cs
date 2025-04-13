using Bcs.API.Endpoints;
using Bcs.Application.Configuration;

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

app.MapEndpoints();

app.Run();
