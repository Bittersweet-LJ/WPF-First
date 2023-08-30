using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace testTreeView.Views.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string imageString)
                {
                    if (string.IsNullOrWhiteSpace(imageString))
                    {
                        return default;
                    }
                    Uri uri = new Uri(imageString, UriKind.Absolute);
                    BitmapSource source = new BitmapImage(uri);
                    return source;
                }
            }
            catch (Exception)
            {
            }
            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
