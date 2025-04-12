using Bcs.Domain.Models.Dto;
using Bcs.Domain.Models.Entity;
using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Application.Mapping;

public static class PlayerMapper
{
    public static PlayerDto MapToPlayerDto(BrawlStarsMember member) => new()
    {
        Tag = member.Tag,
        Name = member.Name,
        Trophies = member.Trophies,
        Role = member.Role
    };
    
    public static PlayerDto MapToPlayerDto(Player player) => new()
    {
        Tag = player.Tag,
        Name = player.Name,
        Trophies = player.Trophies,
        Role = player.Role
    };
    
    public static Player MapToPlayer(BrawlStarsMember player) => new()
    {
        Tag = player.Tag,
        Name = player.Name,
        Role = player.Role,
        CreatedAt = DateTime.UtcNow
    };
    
    public static PlayerDetailsDto MapToPlayerDetailsDto(Player player) => new()
    {
        Tag = player.Tag,
        Name = player.Name,
        Trophies = player.Trophies,
        Role = player.Role,
        ClubTag = player.Tag,
        ClubName = player.Club != null ? player.Club.Name : string.Empty
    };
    
    public static PlayerRankingDto MapToPlayerRankingDto(Player player, int rank) => new()
    {
        Rank = rank,
        Tag = player.Tag,
        Name = player.Name,
        Trophies = player.Trophies,
        ClubTag = player.ClubTag,
        ClubName = player.Club != null ? player.Club.Name : string.Empty,
    };
}