using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace TailoringCompany.Helpers;

public class DateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string dateString)
        {
            DateTime date;
            if (DateTime.TryParse(dateString, null, DateTimeStyles.RoundtripKind, out date))
            {
                return date.ToString("dd MMM yyyy HH:mm"); 
            }
            else if (DateTime.TryParseExact(dateString, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz",
                                            CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("dd MMM yyyy HH:mm");
            }
        }
        return value; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
