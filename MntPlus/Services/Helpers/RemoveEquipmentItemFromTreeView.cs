using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class RemoveEquipmentItemFromTreeView
    {
        public ObservableCollection<EquipmentItemViewModel>? EquipmentTreeViewItems { get; set; }

        public RemoveEquipmentItemFromTreeView(ObservableCollection<EquipmentItemViewModel>? equipmentTreeViewItems)
        {
            EquipmentTreeViewItems = equipmentTreeViewItems; 
        }

        public void RemoveItemFromTreeView(EquipmentItemViewModel? itemToRemove)
        {
            if (itemToRemove is null) return;
            // Find the parent of the item to remove
            var parentViewModel = FindParentViewModel(itemToRemove);

            if (parentViewModel is not null)
            {
                parentViewModel?.Children?.Remove(itemToRemove);
            }
            else
            {
                EquipmentTreeViewItems?.Remove(itemToRemove);
            }
        }
        private EquipmentItemViewModel? FindParentViewModel(EquipmentItemViewModel? itemToRemove)
        {
            if (itemToRemove is null) return null;

            // Search for the parent view model of the item to remove
            foreach (var itemViewModel in EquipmentTreeViewItems!)
            {
                if (itemViewModel is null) continue;
                var parentViewModel = FindParentViewModelRecursive(itemViewModel, itemToRemove);
                if (parentViewModel is not null)
                {
                    return parentViewModel;
                }
            }

            return null; // Parent not found
        }

        private EquipmentItemViewModel? FindParentViewModelRecursive(EquipmentItemViewModel? viewModel, EquipmentItemViewModel? itemToRemove)
        {
            if (itemToRemove is null) return null;

            // Recursively search for the parent view model of the item to remove
            if (viewModel!.Children!.Contains(itemToRemove))
            {
                return viewModel;
            }

            foreach (var childViewModel in viewModel.Children)
            {
                var parentViewModel = FindParentViewModelRecursive(childViewModel, itemToRemove);
                if (parentViewModel != null)
                {
                    return parentViewModel;
                }
            }

            return null;
        }

    }
}
