using Bcs.Application.Mapping;
using Bcs.DataAccess;
using Bcs.Domain.Models.Entity;
using Bcs.Integration.BrawlStars;
using Bcs.Integration.BrawlStars.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bcs.Application.Commands;

public record SynchronizeClubCommand(string Tag) : IRequest<bool>;

internal class SynchronizeClubCommandHandler : IRequestHandler<SynchronizeClubCommand, bool>
{
    private readonly IBrawlStarsService _brawlStarsService;
    private readonly IApplicationDbContext _applicationDbContext;

    public SynchronizeClubCommandHandler(IBrawlStarsService brawlStarsService, IApplicationDbContext applicationDbContext)
    {
        _brawlStarsService = brawlStarsService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> Handle(SynchronizeClubCommand request, CancellationToken cancellationToken)
    {
        var club = await _brawlStarsService.GetClubAsync(request.Tag, cancellationToken)
            ?? throw new Exception($"Failed to retrieve club with tag: {request.Tag}");

        await using var transaction = await _applicationDbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var clubEntity = await _applicationDbContext.Clubs
                .Include(c => c.Players)
                .Include(c => c.ClubStats)
                .Include(c => c.LatestStats)
                .FirstOrDefaultAsync(c => c.Tag == club.Tag, cancellationToken);

            if (clubEntity == null)
            {
                clubEntity = ClubMapper.MapToClub(club);
                _applicationDbContext.Clubs.Add(clubEntity);
            }

            var newPlayerTags = club.Members
                .Select(m => m.Tag)
                .ToHashSet();

            var currentPlayerTags = clubEntity.Players
                .Select(p => p.Tag)
                .ToHashSet();

            var allPlayers = await AddPlayersToClub(newPlayerTags, club, clubEntity);
            RemovePlayersFromClub(newPlayerTags, clubEntity);

            await UpdateClub(clubEntity, club);
            await UpdatePlayers(allPlayers, club);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new Exception($"Failed to synchronize club: {ex.Message}", ex);
        }
    }
    
    private void RemovePlayersFromClub(HashSet<string> newPlayerTags, Club clubEntity)
    {
        var playersToRemove = clubEntity.Players
            .Where(p => !newPlayerTags.Contains(p.Tag))
            .ToList();

        foreach (var player in playersToRemove)
        {
            player.ClubTag = null;
        }
    }
    
    private async Task<List<Player>> AddPlayersToClub(HashSet<string> playerTagsInClub, BrawlStarsClub club, Club clubEntity)
    {
        var existingPlayers = await _applicationDbContext.Players
            .Where(p => playerTagsInClub.Contains(p.Tag))
            .ToListAsync();

        var existingPlayerTags = existingPlayers
            .Select(p => p.Tag)
            .ToHashSet();

        foreach (var player in existingPlayers)
        {
            player.ClubTag = clubEntity.Tag;
        }

        var newPlayers = club.Members
            .Where(m => !existingPlayerTags.Contains(m.Tag))
            .Select(PlayerMapper.MapToPlayer)
            .ToList();

        _applicationDbContext.Players.AddRange(newPlayers);

        return [..newPlayers, ..existingPlayers];
    }

    private async Task UpdateClub(Club clubEntity, BrawlStarsClub club)
    {
        clubEntity.Name = club.Name;
        clubEntity.Description = club.Description;
        clubEntity.RequiredTrophies = club.RequiredTrophies;
        clubEntity.Type = club.Type;
        clubEntity.BadgeId = club.BadgeId;
        
        var newClubStats = new ClubStats
        {
            ClubTag = clubEntity.Tag,
            Trophies = club.Trophies,
            CreatedAt = DateTime.UtcNow
        };
        
        _applicationDbContext.ClubStats.Add(newClubStats);
        
        await _applicationDbContext.SaveChangesAsync();
        clubEntity.LatestStatsId = newClubStats.Id;
    }
    
    private async Task UpdatePlayers(List<Player> players, BrawlStarsClub club)
    {
        foreach (var player in players)
        {
            var brawlStarsPlayer = club.Members.First(p => p.Tag == player.Tag);
            player.Name = brawlStarsPlayer.Name;
            player.Role = brawlStarsPlayer.Role;
        }
        

        await _applicationDbContext.SaveChangesAsync();
        
        var newPlayerStats = players
            .Select(p =>
            {
                var brawlStarsPlayer = club.Members.First(m => m.Tag == p.Tag);
                return new PlayerStats
                {
                    PlayerTag = p.Tag,
                    Trophies = brawlStarsPlayer.Trophies
                };
            })
            .ToList();
        
        _applicationDbContext.PlayerStats.AddRange(newPlayerStats);
        

        var newPlayerSynchronizationHistory = new PlayerSynchronizationHistory()
        {
            CreatedAt = DateTime.UtcNow,
            PlayerStats = newPlayerStats
        };
        
        _applicationDbContext.PlayerSynchronizationHistories.Add(newPlayerSynchronizationHistory);
        
        await _applicationDbContext.SaveChangesAsync();
        foreach (var player in players)
        {
            player.LatestStatsId = newPlayerStats
                .First(ps => ps.PlayerTag == player.Tag)
                .Id;
        }
    }
}