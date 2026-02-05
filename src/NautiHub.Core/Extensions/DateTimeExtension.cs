namespace NautiHub.Core.Extensions;

public static class DateTimeExtension
{
    public static DateTime NextSunday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Sunday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextMonday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Monday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextTuesday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Tuesday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextWednesday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Wednesday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextThursday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Thursday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextFriday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Friday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateTime NextSaturday(this DateTime dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Saturday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static bool ThisIsInTheSameMonthThat(this DateTime data, DateTime outraData)
    {
        return data.Month == outraData.Month &&
               data.Year == outraData.Year;
    }

    public static DateTime ToTimeZone(this DateTime dateTimeUtc, string timeZoneId)
    {
        if (dateTimeUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("O DateTime precisa estar em UTC.", nameof(dateTimeUtc));

        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, tz);
    }
}
