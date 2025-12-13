using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EGO_Library.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = Path.Combine(baseDir, imagePath);

                    var bitmap = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                    return bitmap;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                 System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}