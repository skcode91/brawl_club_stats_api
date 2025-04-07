using System.Net.Http.Headers;
using Bcs.Integration.BrawlStars.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace Bcs.Integration.BrawlStars.Configuration;

public static class BrawlStarsApiIntegrationFeature
{
    public static void Setup(WebApplicationBuilder builder)
    {
        builder.Services.Configure<BrawlStarsClientSettings>(
            builder.Configuration.GetSection(nameof(BrawlStarsClientSettings)));

        builder.Services.AddHttpClient("BrawlStars", (serviceProvider, client) =>
        {
            var settings = serviceProvider
                .GetRequiredService<IOptions<BrawlStarsClientSettings>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", settings.BearerToken);
        });
    }
}