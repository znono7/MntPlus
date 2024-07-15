using System.Windows;
using System.Windows.Controls;

namespace MntPlus.WPF
{
    public class FilterTemplateSelector : DataTemplateSelector
    {
        public  DataTemplate DateRangeTemplate { get; set; }
        public  DataTemplate EmptyTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is FilterPmViewModel filter)
            {

                switch (filter.Field)
                {
                    case "Prochaine échéance":
                        return DateRangeTemplate;
                    case "Empty":
                        return EmptyTemplate;
                    default:
                        return EmptyTemplate;
                }
            }

            return base.SelectTemplate(item, container);
          
        }
    }
}
