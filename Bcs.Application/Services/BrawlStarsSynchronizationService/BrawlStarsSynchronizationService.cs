using Bcs.Application.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Bcs.Application.Configuration.Settings;
using Microsoft.Extensions.DependencyInjection;

public class BrawlStarsSynchronizationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BrawlStarsSynchronizationService> _logger;
    private readonly BrawlStarsSynchronizationServiceSettings _settings;

    public BrawlStarsSynchronizationService(
        IServiceProvider serviceProvider, 
        ILogger<BrawlStarsSynchronizationService> logger,
        BrawlStarsSynchronizationServiceSettings settings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using AsyncServiceScope scope = _serviceProvider.CreateAsyncScope();
                        
            var now = DateTime.Now;
            var nextFullHour = new DateTime(now.Year, now.Month, now.Day, now.Hour + 1, 0, 0);
            var timeToWait = nextFullHour - now;

            _logger.LogInformation("Waiting until the next full hour: {nextFullHour}", nextFullHour);

            await Task.Delay(timeToWait, stoppingToken);
            await TrySynchronizeClubs(scope, _settings.ClubTags, stoppingToken);
        }
    }

    private async Task TrySynchronizeClubs(IServiceScope scope,List<string> tags, CancellationToken stoppingToken)
    {
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        foreach (var tag in tags)
        {
            var success = false;
            while (!success && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await mediator.Send(new SynchronizeClubCommand(tag), stoppingToken);
                    _logger.LogInformation("Club {tag} synchronized", tag);
                    success = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing club {tag}", tag);

                    _logger.LogInformation("Retrying in 1 minute...");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }
    }
}
