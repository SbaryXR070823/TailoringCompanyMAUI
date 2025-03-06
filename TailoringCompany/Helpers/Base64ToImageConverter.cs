using System.Globalization;
using System.IO;
using Microsoft.Maui.Controls;

namespace TailoringCompany.Helpers;

public class Base64ToImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string base64String && !string.IsNullOrEmpty(base64String))
        {
            try
            {
                var base64Data = base64String.Contains(",") ? base64String.Split(',')[1] : base64String;
                byte[] imageBytes = System.Convert.FromBase64String(base64Data);
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
