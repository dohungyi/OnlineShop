﻿namespace SharedKernel.Libraries;

public static class DateHelper
{
    public static DateTime Now => DateTime.UtcNow;

    public static long GetTotalMilliseconds(DateTime date)
    {
        return Convert.ToInt64(date.ToLocalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
    }

    public static DateTime ConvertMillisecondsToDate(long milliseconds)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(milliseconds).ToLocalTime();
    }
}