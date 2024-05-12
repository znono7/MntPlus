
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Shared;

namespace MntPlus.WPF
{
    public class DataGridSelectionChangedBehavior
    {
        public static ICommand GetSelectionChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetSelectionChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
        DependencyProperty.RegisterAttached(
                       "SelectionChangedCommand",
                                  typeof(ICommand),
                                             typeof(DataGridSelectionChangedBehavior),
                                                        new PropertyMetadata(null, OnSelectionChangedCommandChanged));

        private static void OnSelectionChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged += (sender, args) =>
                {
                    if (e.NewValue is ICommand command && command.CanExecute(dataGrid.SelectedItem))
                    {
                        command.Execute(dataGrid.SelectedItem);

                        if (dataGrid.SelectedItem is UserDto client && dataGrid.DataContext is UsersCmbViewModel viewModel)
                        {
                            //viewModel.HandleSelectedItem(client);
                        }

                    }
                };
            }
        }
    }
}
