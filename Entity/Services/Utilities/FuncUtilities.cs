using System.Globalization;


namespace Entity.Services.Utilities
{
    public class FuncUtilities
    {
        public static readonly string[] formatDate = new string[] { "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yyy HH:mm:ss", "MM/dd/yyyy", "yyyy/MM/dd", "MM-dd-yyyy", "M-d-yyyy", "yyyy-MM-dd", "yyyy/M/d" };
       
        public static DateTime ConvertStringToDateTime(string date = "")
        {
            _ = new DateTime();
            DateTime d;
            if (date.Split('-').Count() > 1)
            {
                if (!string.IsNullOrEmpty(date))
                {
                    d = DateTime.ParseExact(date, formatDate, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    d = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                return d;
            }
            if (!string.IsNullOrEmpty(date))
            {
                d = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                d = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            return d;
        }
        public static DateTime ConvertStringToDate(string date="")
        {
            _ = new DateTime();
            DateTime d;
            if (!string.IsNullOrEmpty(date))
            {
                d = DateTime.ParseExact(date, formatDate, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                d = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            return d;
        }
        public static string ConvertDateToString(DateTime date)
        {
            var dateString = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return dateString;
        }
        public static DateTime ConvertToTimeStamp(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static bool BeAValidDate(string DateOfBirth)
        {
            if (DateTime.TryParseExact(DateOfBirth,
                       formatDate,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None
                       , out _) && FuncUtilities.ConvertStringToDate(DateOfBirth) <= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public static bool BeAValidDateOfExpiry(string DateOfExpiry)
        {
            if (!DateTime.TryParseExact(DateOfExpiry,
                       formatDate,
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None
                       , out _))
            {
                return false;
            }
            return true;
        }

    }
}