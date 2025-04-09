using System.Reflection;
using Bcs.Application.Configuration.Settings;
using Bcs.DataAccess;
using Bcs.Integration.BrawlStars;
using Bcs.Integration.BrawlStars.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bcs.Application.Configuration;

public static class DomainModule
{
    public static void SetupDomain(WebApplicationBuilder builder)
    {
        SetupDbContext(builder);
        SetupIntegrations(builder);
        SetupMediatR(builder);
        
        AddHostedServices(builder);
    }

    private static void SetupDbContext(WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbContextSettings>(
            builder.Configuration.GetSection(nameof(DbContextSettings)));

        var dbContextSettings = builder.Configuration
            .GetSection(nameof(DbContextSettings))
            .Get<DbContextSettings>();

        if (string.IsNullOrWhiteSpace(dbContextSettings?.ConnectionString))
        {
            throw new InvalidOperationException(
                "Invalid connection string in configuration. Please check your settings.");
        }

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dbContextSettings.ConnectionString);
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    }
    
    private static void SetupIntegrations(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBrawlStarsService, BrawlStarsService>();

        BrawlStarsApiIntegrationFeature.Setup(builder);
    }
    
    private static void SetupMediatR(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
    
    private static void AddHostedServices(WebApplicationBuilder builder)
    {
        var settings = builder.Configuration
            .GetSection(nameof(BrawlStarsSynchronizationServiceSettings))
            .Get<BrawlStarsSynchronizationServiceSettings>();
        
        if (settings?.ClubTags == null || !settings.ClubTags.Any())
        {
            throw new InvalidOperationException(
                "Invalid BrawlStarsSynchronizationServiceSettings in configuration. Please check your settings.");
        }
    
        builder.Services.AddHostedService<BrawlStarsSynchronizationService>(provider =>
            new BrawlStarsSynchronizationService(
                provider.GetRequiredService<IServiceProvider>(),
                provider.GetRequiredService<ILogger<BrawlStarsSynchronizationService>>(),
                settings));
    }
}