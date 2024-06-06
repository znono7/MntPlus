using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace MntPlus.WPF
{
    public class DoubleConventer : BaseValueConverter<DoubleConventer>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
            if (value is double doubleValue)
        {
                return doubleValue.ToString();
            }
            return value;
        } 

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
            if (value is string input)
        {
                if (string.IsNullOrWhiteSpace(input))
            {
                    return 0; // Default value when TextBox is empty
                }
                if (double.TryParse(input, out double result))
            {
                    return result;
                }
            }
            return 0;
        }
    }
   
}
