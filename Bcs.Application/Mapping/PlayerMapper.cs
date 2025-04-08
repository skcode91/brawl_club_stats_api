using Bcs.Domain.Models.Dto;
using Bcs.Domain.Models.Entity;
using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Application.Mapping;

public static class PlayerMapper
{
    public static PlayerDto MapToPlayerDto(BrawlStarsMember member) => new PlayerDto()
    {
        Tag = member.Tag,
        Name = member.Name,
        Trophies = member.Trophies,
        Role = member.Role
    };
    
    public static Player MapToPlayer(BrawlStarsMember player) => new Player()
    {
        Tag = player.Tag,
        Name = player.Name,
        Role = player.Role,
        CreatedAt = DateTime.UtcNow
    };
}