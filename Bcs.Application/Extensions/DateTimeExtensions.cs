namespace Bcs.Application.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToUtcTruncatedToHour(this DateTime localDateTime)
    {
        var utc = localDateTime.ToUniversalTime();

        return new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0, DateTimeKind.Utc);
    }
    
    public static DateTime ToUtcTruncatedToNextHour(this DateTime localDateTime)
    {
        var utc = localDateTime.ToUniversalTime();

        return new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour + 1, 0, 0, DateTimeKind.Utc);
    }
    
    public static DateTime GetFirstHourOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
    }
    
    public static DateTime GetFirstHourOfNextDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind).AddDays(1);
    }
}