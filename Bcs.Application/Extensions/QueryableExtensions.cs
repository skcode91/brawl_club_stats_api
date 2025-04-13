using Bcs.Domain.Models.Entity;

namespace Bcs.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereCreatedAtInHour<T>(
        this IQueryable<T> query,
        DateTime localDateTimeHour)
        where T : TimeStampedEntity
    {
        var fromUtc = localDateTimeHour.ToUtcTruncatedToHour();
        var toUtc = localDateTimeHour.ToUtcTruncatedToNextHour();

        return query.Where(x => x.CreatedAt >= fromUtc && x.CreatedAt < toUtc);
    }
    
    public static IQueryable<T> WhereCreatedAtInDay<T>(
        this IQueryable<T> query,
        DateTime localDateTime)
        where T : TimeStampedEntity
    {
        var fromUtc = localDateTime.GetFirstHourOfDay().ToUtcTruncatedToHour();
        var toUtc = localDateTime.GetFirstHourOfNextDay().ToUtcTruncatedToHour();

        return query.Where(x => x.CreatedAt >= fromUtc && x.CreatedAt < toUtc);
    }
}