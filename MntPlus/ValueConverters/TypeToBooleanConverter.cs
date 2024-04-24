using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MntPlus.WPF
{
    public class TypeToBooleanConverter : BaseValueConverter<TypeToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return parameter?.ToString();
            return Binding.DoNothing;
        }
    }
   
}
