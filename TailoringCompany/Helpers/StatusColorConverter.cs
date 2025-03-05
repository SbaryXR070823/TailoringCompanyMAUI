using System;
using System.Globalization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Shared.Enums;

namespace TailoringCompany.Helpers;

public class StatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status && Enum.TryParse(status, out StatusEnum statusEnum))
        {
            return statusEnum switch
            {
                StatusEnum.Placed => Colors.Gray,
                StatusEnum.OfferPlaced => Colors.Blue,
                StatusEnum.Accepted => Colors.Green,
                StatusEnum.InProgress => Colors.Orange,
                StatusEnum.Finished => Colors.Purple,
                StatusEnum.PickedUp => Colors.DarkGreen,
                StatusEnum.Declined => Colors.Red,
                _ => Colors.Gray
            };
        }
        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
