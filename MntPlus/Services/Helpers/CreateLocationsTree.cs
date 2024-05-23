using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class CreateLocationsTree
    {
        public ObservableCollection<LocationItemViewModel> CreateTreeViewItems(List<LocationDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<LocationItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData?.Where(e => e.IdParent == null);

            foreach (var rootItem in rootItems!) 
            {
                var rootViewModel = new LocationItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);



                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }

        private void CreateTreeViewChildren(LocationItemViewModel parentViewModel, List<LocationDto>? equipmentData)
        {
           
            var children = equipmentData?.Where(e => parentViewModel != null 
                                                    && parentViewModel?.LocationDto != null  
                                                    &&  e.IdParent == parentViewModel?.Id);

            if(children == null) return;
         
            foreach (var child in children)
            {
                var childViewModel = new LocationItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel?.Children?.Add(childViewModel);
            }
        }

       
    }
}
