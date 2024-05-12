using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class TagControlViewModel : BaseViewModel
    {
        public EquipmentFilterType EquipmentFilterType { get; set; }
        public string? TagName { get; set; }
        public string? FilterValue { get; set; }
        public ICommand CancelTagCommand { get; set; }
        public Func<EquipmentFilterType, Task>? CancelTagFonction { get; set; }
        public TagControlViewModel(string? tagName, string? filterValue, EquipmentFilterType equipmentFilterType)
        {
            CancelTagCommand = new RelayCommand(async () => await CancelTag());
            TagName = tagName;
            FilterValue = filterValue;
            EquipmentFilterType = equipmentFilterType;
        }

        private async Task CancelTag()
        {
            if (CancelTagFonction != null && TagName != null)
            {
                await CancelTagFonction(this.EquipmentFilterType);
            }
        }
    }
}
