using MntPlus.WPF.ViewModels.Windows;
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
    public class EquipmentPartViewModel : BaseViewModel
    {
        public ICommand AddEquipmentPartCommand { get; set; }
        public ObservableCollection<PartInventoryItem>? PartItemViewModels { get; set; }
       public PartStore PartStore { get; set; }
        public EquipmentPartViewModel()
        {
            AddEquipmentPartCommand = new RelayCommand(AddEquipmentPart);
            PartStore = new PartStore();
            PartStore.PartCreated += PartStore_PartAdded;
        }

        private void PartStore_PartAdded(PartDto? dto)
        {
            PartItemViewModels ??= new ObservableCollection<PartInventoryItem>();
            PartItemViewModels?.Add(new PartInventoryItem(dto));
            
        }

        private void AddEquipmentPart()
        {
            SelectPartWindow selectPartWindow = new() { DataContext = new SelectPartViewModel(PartStore) };
            selectPartWindow.ShowDialog();
        }
    }
}
