using System.Globalization;

namespace Net.Gwiasda.Local.UI.ViewModel
{
    public abstract class ViewModel
    {
        protected const string DATE_FORMAT = "yyyyMMddHHmm";
        protected DateTime ParseDateTime(string dateTime)
        {
            if(!DateTime.TryParseExact(dateTime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                throw new ArgumentException($"Invalid date format '{dateTime}'.", nameof(dateTime));

            return result;
        }
    }
}