namespace NASAData_API.Helpers
{
    static public class DataTimeExtension
    {
        public static string ParseYearToDateTime(string year)
        {
            DateTime dateYear = DateTime.ParseExact(year, "yyyy", null);
            return dateYear.ToString("yyyy-MM-dd 00:00:00");
        }

        public static string ParseDateTimeToYear(string dateTime)
        {
            DateTime dateYear = DateTime.Parse(dateTime);
            return dateYear.ToString("yyyy");
        }
    }
}
