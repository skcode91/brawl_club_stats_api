using Bcs.Domain.Models.Dto;
using Bcs.Domain.Models.Entity;
using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Application.Mapping;

public static class ClubMapper
{
    public static ClubDto MapToClubDto(BrawlStarsClub club) => new()
    {
        Tag = club.Tag,
        Name = club.Name,
        Description = club.Description,
        Trophies = club.Trophies,
        RequiredTrophies = club.RequiredTrophies,
        Type = club.Type,
        Members = club.Members.Select(PlayerMapper.MapToPlayerDto).ToList()
    };
    
    public static ClubDto MapToClubDto(Club club) => new()
    {
        Tag = club.Tag,
        Name = club.Name,
        Description = club.Description,
        Trophies = club.LatestStats.Trophies,
        RequiredTrophies = club.RequiredTrophies,
        Type = club.Type,
        Members = club.Players.Select(PlayerMapper.MapToPlayerDto).OrderByDescending(x => x.Trophies).ToList()
    };

    public static Club MapToClub(BrawlStarsClub club) => new()
    {
        Tag = club.Tag,
        Name = club.Name,
        Description = club.Description,
        RequiredTrophies = club.RequiredTrophies,
        Type = club.Type,
        CreatedAt = DateTime.UtcNow
    };
}
 