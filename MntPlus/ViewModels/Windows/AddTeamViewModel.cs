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
    public class AddTeamViewModel : BaseViewModel
    {
        public string? Name { get; set; }
      
        public UsersCmbViewModel UsersCmbViewModel { get; set; }


        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }

        public AddTeamViewModel()
        {
            UsersCmbViewModel = new UsersCmbViewModel();
            SaveCommand = new RelayParameterizedCommand(async (parameter) => await SaveAsync(parameter));
           
        }

        private async Task SaveAsync(object? parameter)
        {
            if(!CanSave())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel( NotificationType.Error,"Please enter a name for the team"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var result = await AppServices.ServiceManager.TeamService.CreateTeam(new TeamCreatedDto(Name!));
                if (result is ApiOkResponse<TeamDto> okResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Équipe créée avec succès"));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création de l'équipe"));
                }
            });
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
