using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Integration.BrawlStars;

public interface IBrawlStarsService
{
    Task<BrawlStarsClub> GetClubAsync(string tag, CancellationToken cancellationToken);
}