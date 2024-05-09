using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SelectEquipmentItemViewModel : BaseViewModel
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public string EquipmentName { get; set; }
        public AssetDto Equipment { get; set; }

        public ObservableCollection<SelectEquipmentItemViewModel> Children { get; set; } 
        public ICommand SelectEquipmentCommand { get; set; }
        public Func<SelectEquipmentItemViewModel, Task> SelectEquipmentFunc { get; set; } 

        public SelectEquipmentItemViewModel(AssetDto equipment) 
        {
            EquipmentName = equipment.Name;
            Equipment = equipment;
            Children = new ObservableCollection<SelectEquipmentItemViewModel>();
            SelectEquipmentCommand = new RelayCommand(async () => await GetSelected());
        }

        private async Task GetSelected()
        {
            await SelectEquipmentFunc(this);
                
        }
    }
}
