using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class CreateEquipmentTreeViewItems
    {
        public ObservableCollection<AssetItemViewModel> CreateTreeViewItems(List<AssetDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<AssetItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData?.Where(e => e.AssetParent is null);

            foreach (var rootItem in rootItems!)
            {
                var rootViewModel = new AssetItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);

                CalculateNumberOfChildren(rootViewModel, equipmentData);


                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }

        private void CreateTreeViewChildren(AssetItemViewModel parentViewModel, List<AssetDto>? equipmentData)
        {
           
            var children = equipmentData?.Where(e => parentViewModel is not null && parentViewModel?.Asset is not null  &&  e.AssetParent == parentViewModel?.Asset?.Id);

            foreach (var child in children!)
            {
                var childViewModel = new AssetItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel?.Children?.Add(childViewModel);
            }
        }

        private void CalculateNumberOfChildren(AssetItemViewModel? parentViewModel, List<AssetDto>? equipmentData)
        {
            if(parentViewModel is null || parentViewModel?.Asset is null)
            {
                return;
            }
            // Calculate the number of children for the parentViewModel
            parentViewModel.ChildrenCount = !(equipmentData is null)
                                            ? equipmentData.Count( e => e.AssetParent == parentViewModel?.Asset?.Id ) : 0;

            // Recursively calculate the number of children for each child
            if (parentViewModel?.Children != null)
            {
                foreach (var childViewModel in parentViewModel.Children)
                {
                    CalculateNumberOfChildren(childViewModel, equipmentData);
                }
            }

        }
    }
}
