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
        
        var clubEntity = await _applicationDbContext.Clubs
            .Include(c => c.Players)
            .Include(c => c.ClubSynchronizationHistories)
            .FirstOrDefaultAsync(c => c.Tag == club.Tag, cancellationToken);

        if (clubEntity == null)
        {
            clubEntity = ClubMapper.MapToClub(club);
            _applicationDbContext.Clubs.Add(clubEntity);
        }

        var newPlayerTags = club.Members
            .Select(m => m.Tag)
            .ToHashSet();

        var allPlayers = await AddPlayersToClub(newPlayerTags, club, clubEntity);
        RemovePlayersFromClub(newPlayerTags, clubEntity);

        UpdateClub(clubEntity, club);
        UpdatePlayers(allPlayers, clubEntity, club);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return true;
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

    private void UpdateClub(Club clubEntity, BrawlStarsClub club)
    {
        clubEntity.Name = club.Name;
        clubEntity.Description = club.Description;
        clubEntity.RequiredTrophies = club.RequiredTrophies;
        clubEntity.Type = club.Type;
        clubEntity.BadgeId = club.BadgeId;
        clubEntity.Trophies = club.Trophies;
    }
    
    private void UpdatePlayers(List<Player> players, Club clubEntity, BrawlStarsClub club)
    {
        foreach (var player in players)
        {
            var brawlStarsPlayer = club.Members.First(p => p.Tag == player.Tag);
            player.Name = brawlStarsPlayer.Name;
            player.Role = brawlStarsPlayer.Role;
            player.Trophies = brawlStarsPlayer.Trophies;
            player.ClubTag = clubEntity.Tag;
        }
        
        var clubSynchronizationHistory = new ClubSynchronizationHistory
        {
            ClubTag = clubEntity.Tag,
            Trophies = clubEntity.Trophies,
            PlayerStats = players
                .Select(p =>
                {
                    var brawlStarsPlayer = club.Members.First(m => m.Tag == p.Tag);
                    return new PlayerStats
                    {
                        PlayerTag = p.Tag,
                        Trophies = brawlStarsPlayer.Trophies,
                        CreatedAt = DateTime.UtcNow
                    };
                })
                .ToList(),
            CreatedAt = DateTime.UtcNow
        };
        
        _applicationDbContext.ClubSynchronizationHistories.Add(clubSynchronizationHistory);
    }
}