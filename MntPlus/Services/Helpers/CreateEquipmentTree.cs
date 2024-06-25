using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class CreateEquipmentTree
    {
        public ObservableCollection<EquipmentItemViewModel> CreateTreeViewItems(List<AssetDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<EquipmentItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData?.Where(e => e.AssetParent == null);

            foreach (var rootItem in rootItems!) 
            {
                var rootViewModel = new EquipmentItemViewModel(rootItem); 
                CreateTreeViewChildren(rootViewModel, equipmentData);

                CalculateNumberOfChildren(rootViewModel, equipmentData);


                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }

        private void CreateTreeViewChildren(EquipmentItemViewModel parentViewModel, List<AssetDto>? equipmentData)
        {
           
            var children = equipmentData?.Where(e => parentViewModel != null 
                                                    && parentViewModel?.Asset != null  
                                                    &&  e.AssetParent == parentViewModel?.Asset?.Id);

            if(children == null) return;
         
            foreach (var child in children)
            {
                var childViewModel = new EquipmentItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel?.Children?.Add(childViewModel);
            }
        }

        private void CalculateNumberOfChildren(EquipmentItemViewModel? parentViewModel, List<AssetDto>? equipmentData)
        {
            if(parentViewModel is null || parentViewModel?.Asset is null)
            {
                return;
            }
            // Calculate the number of children for the parentViewModel
            parentViewModel.ChildrenCount = !(equipmentData == null)
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
