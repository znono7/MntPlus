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
    public class AssignNewRoleViewModel : BaseViewModel
    {
        public List<RoleDto?>? RoleDtos { get; set; }
        public RoleDto? SelectedRole { get; set; } = null;
        public ICommand AddCommand { get; set; }
        public bool SaveIsRunning { get; set; }
        public Guid UserId { get; }
        public RoleStore? RoleStore { get; set; }

        public AssignNewRoleViewModel(Guid userId , RoleStore? roleStore , List<RoleDto?>? roleDtos)
        {
            _ = GetRoles();
            AddCommand = new RelayParameterizedCommand(async (p) => await SaveRole(p));
            UserId = userId;
            RoleStore = roleStore;
            RoleDtos = roleDtos;
        }

        private async Task SaveRole(object? p)
        {
            if (p == null) { return; }
            if (SelectedRole is null)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Remplissez tous les champs, s'il vous plaît"));
                return;
            }
            if(RoleDtos != null &&  RoleDtos.Any(m => m.Id == SelectedRole.Id))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le rôle existe déjà"));
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var Result = await AppServices.ServiceManager.UserRoleService.CreateUserRoles(new RoleForUserCreationDto(UserId, SelectedRole.Id));
                if ( Result is ApiOkResponse<UserRoleDto> result )
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Rôle ajouté avec succès"));
                    RoleStore?.AssignRole(result.Result);
                    var Window = p as AssignNewRole;
                    Window?.Close();
                }
                else if (Result is not null && Result is ApiBadRequestResponse error)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, error.Message));
                }
               
            });
        }

        private async Task GetRoles()
        {
            var roles = await AppServices.ServiceManager.RoleService.GetAllRolesAsync(false);
            if (roles is not null && roles is ApiOkResponse<IEnumerable<RoleDto>> result && result.Result is not null)
            {
                RoleDtos = new List<RoleDto>(result.Result);
            }
            else if (roles is not null && roles is ApiBadRequestResponse error)
            {
                RoleDtos = new List<RoleDto>();

                //await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, error.Message));
            }
            else if (roles is not null && roles is ApiNotFoundResponse)
            {
                RoleDtos = new List<RoleDto>();
                //await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Aucun rôle trouvé"));
            }

        }
    }
}
