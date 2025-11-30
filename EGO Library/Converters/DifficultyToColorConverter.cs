using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EGO_Library.Converters
{
    public class DifficultyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string difficulty)
            {
                return difficulty switch
                {
                    "Easy" => new SolidColorBrush(Colors.LightGreen),
                    "Medium" => new SolidColorBrush(Colors.Orange),
                    "Hard" => new SolidColorBrush(Colors.OrangeRed),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}