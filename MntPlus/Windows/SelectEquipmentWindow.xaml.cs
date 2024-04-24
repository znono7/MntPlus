using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MntPlus.WPF
{
    /// <summary>
    /// Interaction logic for SelectEquipmentWindow.xaml
    /// </summary>
    public partial class SelectEquipmentWindow : Window 
    {
       
        public SelectEquipmentWindow() 
        {
            InitializeComponent();
            ExpandAllTreeViewItems(MyTreeView);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Method to recursively expand all tree items
        private void ExpandAllTreeViewItems(TreeView treeView)
        {
            foreach (var item in treeView.Items)
            {
                if (item is TreeViewItem treeViewItem)
                {
                    // Expand current item
                    treeViewItem.IsExpanded = true;

                    // Recursively expand child items
                    ExpandAllTreeViewItems(treeViewItem);
                }
            }
        }

        // Method to recursively expand all child items of a TreeViewItem
        private void ExpandAllTreeViewItems(TreeViewItem treeViewItem)
        {
            foreach (var item in treeViewItem.Items)
            {
                if (item is TreeViewItem childTreeViewItem)
                {
                    // Expand current child item
                    childTreeViewItem.IsExpanded = true;

                    // Recursively expand child items
                    ExpandAllTreeViewItems(childTreeViewItem);
                }
            }
        }

        private SelectEquipmentItemViewModel? GetSelectedViewModel(TreeViewItem root)
        {
            if (root == null)
                return null;

            foreach (var item in root.Items)
            {
                var treeItem = root.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem != null)
                {
                    var viewModel = treeItem.DataContext as SelectEquipmentItemViewModel;
                    if (viewModel != null && viewModel.IsSelected)
                    {
                        return viewModel;
                    }

                    // Check children recursively
                    var selectedChild = GetSelectedViewModel(treeItem);
                    if (selectedChild != null)
                    {
                        return selectedChild;
                    }
                }
            }

            return null;
        }

       

      
    }
}
