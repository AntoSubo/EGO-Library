using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EGO_Library.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    string debugPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

                    string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                    string projectPath = Path.Combine(projectRoot, imagePath);

                    string finalPath = File.Exists(debugPath) ? debugPath :
                                       File.Exists(projectPath) ? projectPath :
                                       null;

                    if (finalPath != null)
                    {
                        var bitmap = new BitmapImage(new Uri(finalPath, UriKind.Absolute));
                        return bitmap;
                    }
                    return null;
                }
                catch { return null; }
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