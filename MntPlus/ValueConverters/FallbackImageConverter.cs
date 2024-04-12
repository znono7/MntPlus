using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class FallbackImageConverter : BaseValueConverter<FallbackImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                // Provide a default image source when the value is null
                return null;//new BitmapImage(new Uri("pack://application:,,,/MntPlus.WPF;Images/DefaultImage.png"));
            }
            else
            {
                // Return the original image source
                return value;
            }

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
