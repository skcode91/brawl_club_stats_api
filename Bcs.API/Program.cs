using Bcs.API.Endpoints;
using Bcs.Application.Configuration;

var builder = WebApplication.CreateBuilder(args);

DomainModule.SetupDomain(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultPolicy",
        builder =>
        {
            builder
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultPolicy");

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
