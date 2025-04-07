using Bcs.Domain.Models.Dto;
using Bcs.Domain.Models.Entity;
using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Application.Mapping;

public static class ClubMapper
{
    public static ClubDto MapToClubDto(Club club) => new ClubDto()
    {
        Id = club.Id,
        Tag = club.Tag,
        Name = club.Name
    };
    
    public static ClubDto MapToClubDto(BrawlStarsClub club) => new ClubDto()
    {
        Id = 0,
        Tag = club.Tag,
        Name = club.Name
    };
}