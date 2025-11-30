using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EGO_Library.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return null;

            string imagePath = value.ToString();

            try
            {
                // Если путь уже в формате pack://, используем как есть
                if (imagePath.StartsWith("pack://"))
                {
                    return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                }

                // Если это относительный путь
                if (File.Exists(imagePath))
                {
                    return new BitmapImage(new Uri(Path.GetFullPath(imagePath)));
                }

                // Пробуем найти в ресурсах
                string resourcePath = $"pack://application:,,,/{imagePath.TrimStart('/')}";
                return new BitmapImage(new Uri(resourcePath, UriKind.Absolute));
            }
            catch
            {
                // Если изображение не найдено, возвращаем null (будет черный квадрат)
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}