using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace CoffeeShopManagement.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public static readonly StringToColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() switch
                {
                    "pending" or "low stock" => Brushes.Orange,
                    "completed" or "in stock" => Brushes.Green,
                    "cancelled" or "out of stock" => Brushes.Red,
                    _ => Brushes.Gray
                };
            }
            return Brushes.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
