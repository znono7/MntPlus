using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class FindEquipmentParentViewModel
    {
        public ObservableCollection<EquipmentItemViewModel>? EquipmentTreeViewItems { get; set; }

        public FindEquipmentParentViewModel(ObservableCollection<EquipmentItemViewModel>? equipmentTreeViewItems)
        {
            EquipmentTreeViewItems = equipmentTreeViewItems;
        }
        public EquipmentItemViewModel? FindParentViewModel(Guid? parentId)
        {
            if (parentId is null)
            {
                return null;
            }

            // Search for the parent view model based on parentId
            foreach (var itemViewModel in EquipmentTreeViewItems!)
            {
                var parentViewModel = FindParentViewModelRecursive(itemViewModel, parentId);
                if (parentViewModel is not null)
                {
                    return parentViewModel;
                }
            }

            return null; // Parent not found
        }

        private EquipmentItemViewModel? FindParentViewModelRecursive(EquipmentItemViewModel? viewModel, Guid? parentId)
        {
            if (viewModel?.Asset?.Id == parentId)
            {
                return viewModel;
            }

            foreach (var childViewModel in viewModel?.Children!)
            {
                var parentViewModel = FindParentViewModelRecursive(childViewModel, parentId);
                if (parentViewModel is not null)
                {
                    return parentViewModel;
                }
            }

            return null;
        }

    }
}
