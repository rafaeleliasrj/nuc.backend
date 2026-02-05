namespace NautiHub.Core.Extensions;

public static class DateOnlyExtension
{
    public static DateOnly NextSunday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Sunday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextMonday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Monday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextTuesday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Tuesday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextWednesday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Wednesday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextThursday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Thursday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextFriday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Friday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }

    public static DateOnly NextSaturday(this DateOnly dateReference)
    {
        while (dateReference.DayOfWeek != DayOfWeek.Saturday)
        {
            dateReference = dateReference.AddDays(1);
        }
        return dateReference;
    }
}
