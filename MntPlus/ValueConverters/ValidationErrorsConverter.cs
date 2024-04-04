using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MntPlus
{
    public class ValidationErrorsConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var errors = values[0] as ReadOnlyObservableCollection<System.Windows.Controls.ValidationError>;
        if (errors == null || errors.Count == 0)
        {
            return null;
        }

        // Concatenate error messages
        return string.Join(" ", errors.Select(error => error.ErrorContent.ToString()));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

}
