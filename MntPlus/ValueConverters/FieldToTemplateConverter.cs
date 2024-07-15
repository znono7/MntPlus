using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MntPlus.WPF
{
    public class FieldToTemplateConverter : IValueConverter
    {
        public DataTemplate DateRangeTemplate { get; set; }
        public DataTemplate EmptyTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filter)
            {

                switch (filter)
                {
                    case "Prochaine échéance":
                        return DateRangeTemplate;
                    case "Empty":
                        return EmptyTemplate;
                    default:
                        return EmptyTemplate;
                }
            }
            return EmptyTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
