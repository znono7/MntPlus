using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class EquipmentHierarchyManager
    {
        private List<EquipmentItemViewModel> HierarchyParents { get; set; } = new List<EquipmentItemViewModel>();
        public List<EquipmentItemViewModel>? EquipmentHierarchy { get; private set; }

        public void GetFullHierarchy(EquipmentItemViewModel item)
        {
            HierarchyParents.Clear();
            GetParentHierarchy(item);

            HierarchyParents.Reverse();
            HierarchyParents.Add(item);
            if (item.Children != null)
            {
                foreach (var child in item.Children)
                {
                    HierarchyParents.Add(child);
                }
            }

            EquipmentHierarchy = new List<EquipmentItemViewModel>(HierarchyParents);
        }

        private void GetParentHierarchy(EquipmentItemViewModel item)
        {
            if (item.Parent != null)
            {
                HierarchyParents.Add( new EquipmentItemViewModel( item.Parent));
                GetParentHierarchy(new EquipmentItemViewModel(item.Parent));
            }
        }

    }
}
