using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class CreateEquipmentListItems
    {
        public ObservableCollection<EquipmentItemViewModel> CreateListItems(List<AssetDto> equipmentData)
        {
            // For the list view, simply convert the equipment data to view models
            return new ObservableCollection<EquipmentItemViewModel>(
                                equipmentData.Select(e => new EquipmentItemViewModel(e)));


        }
    }
}
