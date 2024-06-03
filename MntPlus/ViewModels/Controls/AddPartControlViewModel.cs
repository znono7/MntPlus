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
    public class AddPartControlViewModel : BaseViewModel
    {
        public string? PartNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }

        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }
        public PartStore PartStore { get; }

        public AddPartControlViewModel(PartStore partStore)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            PartStore = partStore;
        }

        private async Task SaveAsync()
        {
            if(string.IsNullOrEmpty(PartNumber) || string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le numéro de pièce et le nom sont requis"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                PartForCreationDto part = new PartForCreationDto
                (
                    PartNumber : PartNumber,
                    Name : Name,
                    Description: Description,
                    Category: Category,
                    null
                );
                var response = await AppServices.ServiceManager.PartService.CreatePart(part);
                if (response.Success && response is ApiOkResponse<PartDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La pièce a été ajoutée avec succès"));
                    PartStore.CreatePart(result.Result);
                    await CloseAsync();
                }
                else if ( response is ApiBadRequestResponse badResp)
           
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResp.Message));
                }
                
                await CloseAsync();
            });
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();

        }
    }
}
