namespace HighBoard.Domain.Helpers;

public static class DateTimeHelper
{
    /// <summary>
    /// İçinde bulunan ayın ilk günü
    /// </summary>
    /// <returns></returns>
    public static DateTime FirstOfCurrentMonth()
    {
        return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 0);
    }

    /// <summary>
    /// Parametre olarak verilen bir tarihin ilk ilk saniyesi
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime ResetTimeToStartOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
    }

    /// <summary>
    /// Parametre olarak verilen bir tarihin son saniyesi
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime ResetTimeToEndOfDay(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
    }
}
