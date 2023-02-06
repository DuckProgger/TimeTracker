namespace Services.Infrastructure;

internal class DateOnlyHelper
{
    public static DateOnly Today()
    {
        var today = DateTime.Today.Date;
        return new DateOnly(today.Year, today.Month, today.Day);
    }
}