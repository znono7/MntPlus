using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class AddInventoryViewModel  : BaseViewModel 
    {
        public int AvailableQuantity { get; set; } = 0;
        public DateTime DateReceived { get; set; } = DateTime.Now;
        public string DateReceivedText => DateReceived.ToShortDateString();
        public double Cost { get; set; } = 0;
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }

        public ICommand OpenDateCommand { get; set; }
        public bool IsMenuDateOpen { get; set; }
        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }
        public InventoryStore? InventoryStore { get; }
        public Guid PartId { get; }

        public AddInventoryViewModel(InventoryStore? inventoryStore,Guid partId)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            InventoryStore = inventoryStore;
            PartId = partId;
            OpenDateCommand = new RelayCommand(() => IsMenuDateOpen = !IsMenuDateOpen);
            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        private async Task SaveAsync()
        {
            if(AvailableQuantity <= 0 || Cost <= 0 )
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La quantité disponible et le coût doivent être supérieurs à 0"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
               var response = await AppServices.ServiceManager.InventoryService.CreateInventory(new Shared.InventoryForCreationDto(
                   null,Cost,AvailableQuantity,0,0,DateReceived,PartId));
                if (response.Success && response is ApiOkResponse<InventoryDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "L'inventaire a été ajouté avec succès"));
                    InventoryStore?.CreateInventory(result.Result);
                    await CloseAsync();
                }
                else if (response is ApiBadRequestResponse badResp)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResp.Message));

                }
            });
           
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();
            
        }
    }

}
