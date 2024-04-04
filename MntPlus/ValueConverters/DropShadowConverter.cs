using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace MntPlus
{
    public class DropShadowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMouseOver && isMouseOver)
            {
                return new DropShadowEffect
                {
                    BlurRadius = 5,
                    ShadowDepth = 5,
                    Color = Colors.LightSlateGray,
                    Opacity = 0.3,
                    
                };
            }
            else
            {
                return null; // No effect when the mouse is not over the control
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    }
