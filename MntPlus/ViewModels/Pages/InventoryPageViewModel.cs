using Entities;
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
    public class InventoryPageViewModel : BaseViewModel
    {
        public ObservableCollection<InventoryDto> Inventories { get; set; } 
        public AddInventoryViewModel AddInventoryViewModel { get; set; } 

        public bool IsAddInventoryVisible { get; set; }
        
        private PartDto? _part { get; set; }
        public PartDto? Part 
        { 
            get => _part; 
            set 
            {
                _part = value;
                PartText = $"Piéce: {value?.Name} - {value?.PartNumber}";
                NumberOfInventoryText = $"{value?.Inventories?.Count} Lignes de stock";
                TotalAvailableText = $"Total Quantité disponible: {value?.Inventories?.Sum(x => x.AvailableQuantity)}";
                Inventories = new ObservableCollection<InventoryDto>(value?.Inventories ?? new List<InventoryDto>());
                OnPropertyChanged(nameof(Part));
            }
        }
        public ICommand BackPageCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public string? PartText { get; set; }
        public string? NumberOfInventoryText { get; set; }
        public string? TotalAvailableText { get; set; }
        public bool DimmableOverlayVisible { get; set; }
        public InventoryStore InventoryStore { get; set; }
        public ICommand DeleteCommand { get; set; }
        public InventoryPageViewModel()
        {
           
            BackPageCommand = new RelayCommand(BackPage);
            AddCommand = new RelayCommand(AddInventory);
            InventoryStore = new InventoryStore();
            InventoryStore.InventoryCreated += InventoryStore_InventoryCreated;
            DeleteCommand = new RelayParameterizedCommand(async(p) => await DeleteInventory(p));
        }

        private async Task DeleteInventory(object? p)
        {
            if (p is not InventoryDto inventory)
                return;
            var response = await AppServices.ServiceManager.InventoryService.DeleteInventory(inventory.Id,false);
            if (response.Success)
            {
                Inventories.Remove(inventory); 

                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "L'inventaire a été supprimé avec succès"));
            }else if (response is ApiBadRequestResponse badResp)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResp.Message));
            }
            
        }

        private void InventoryStore_InventoryCreated(InventoryDto? dto)
        {
            Inventories.Add(dto!);
            NumberOfInventoryText = $"{Inventories?.Count} Lignes de stock";
            TotalAvailableText = $"Total Quantité disponible: {Inventories?.Sum(x => x.AvailableQuantity)}";

        }

        private void AddInventory()
        {
            AddInventoryViewModel = new (InventoryStore,Part.Id);
            AddInventoryViewModel.CloseAction = async () =>
            {
                IsAddInventoryVisible = false;
                DimmableOverlayVisible = false;
                await Task.Delay(1);
            };
            IsAddInventoryVisible = true;
            DimmableOverlayVisible = true;
        }

        private void BackPage()
        {
            IoContainer.Application.GoToPage(ApplicationPage.PartsInventory);
        }
    }
}
