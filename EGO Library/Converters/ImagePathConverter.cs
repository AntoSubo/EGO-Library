using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EGO_Library.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                try
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = Path.Combine(baseDir, imagePath);

                    // пробуем как файл в файловой системе
                    if (File.Exists(fullPath))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad; 
                        bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                        bitmap.EndInit();
                        return bitmap;
                    }

                    // Вариант 1: Для контента (Content)
                    string relativePath = imagePath.Replace('\\', '/');
                    if (!relativePath.StartsWith("/"))
                        relativePath = "/" + relativePath;

                    Uri uri;


                    //  application для embedded ресурсов
                    uri = new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component{relativePath}");
                    try
                    {
                        return new BitmapImage(uri);
                    }
                    catch { }

                    // Последняя попытка: относительный путь
                    try
                    {
                        return new BitmapImage(new Uri(imagePath, UriKind.Relative));
                    }
                    catch { }

                    Console.WriteLine($"Image not found: {imagePath}. Searched at: {fullPath}");
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ImagePathConverter error for '{imagePath}': {ex.Message}");
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
}