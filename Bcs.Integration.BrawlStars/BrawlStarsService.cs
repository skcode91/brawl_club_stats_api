using System.Net;
using Bcs.Integration.BrawlStars.Models;

namespace Bcs.Integration.BrawlStars;

public class BrawlStarsService(IHttpClientFactory httpClientFactory) : IBrawlStarsService
{
    public async Task<BrawlStarsClub> GetClubAsync(string tag, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("BrawlStars");

        var encodedTag = WebUtility.UrlEncode(tag);
        var response = await client.GetAsync($"clubs/%23{encodedTag}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Brawl Stars API error: {response.StatusCode}");
        }

        var club = await response.Content.ReadFromJsonAsync<BrawlStarsClub>(cancellationToken: cancellationToken);
        return club ?? throw new Exception("Failed to parse club info.");
    }
}