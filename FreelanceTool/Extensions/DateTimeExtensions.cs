namespace System
{
    public static class DateTimeExtensions
    {
	    public static string ToStringLocale(this DateTime datetime)
	    {
		    return datetime.ToString("dd.MM.yyyy");
	    }
    }
}
