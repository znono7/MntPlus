using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class EquipmentChildrenViewModel : BaseViewModel
    {
        public EquipmentChildrenViewModel(ObservableCollection<EquipmentItemViewModel> _children)
        {
            Children = _children;
        }

        public ObservableCollection<EquipmentItemViewModel> Children { get; }
    }
}
