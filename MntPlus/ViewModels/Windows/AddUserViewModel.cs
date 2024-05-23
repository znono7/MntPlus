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
    public class AddUserViewModel : BaseViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Status { get; set; } = "Actif"; // "actif" or "inactif
        public DateTime? CreatedAt => DateTime.Now;

        public List<RoleDto>? RoleDtos { get; set; }
        public RoleDto? SelectedRole { get; set; } = null;
      

        public bool SaveIsRunning { get; set; }
        public ICommand AddCommand { get; set; }
        public UserStore? UserStore { get; }

        public AddUserViewModel(UserStore? userStore)
        {
            AddCommand = new RelayParameterizedCommand(async(p)=> await SaveUser(p));
            UserStore = userStore;
            //_ = GetRoles();
        }

        private async Task SaveUser(object? p)
        {
            if (p == null) { return;}
            if(!CanSave() /*|| string.IsNullOrEmpty((p as IHavePassword).SecurePassword.Unsecure())*/) 
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Remplissez tous les champs, s'il vous plaît"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var Result = await AppServices.ServiceManager.UserService.CreateUser(
                    new UserCreateDto(FirstName, LastName, Email, PhoneNumber, 
                    UserName, (p as IHavePassword).SecurePassword.Unsecure()??"_", Status, CreatedAt));
                if(Result is not null && Result is ApiOkResponse<UserDto> result && result.Result is not null)
                {  
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Utilisateur ajouté avec succès"));
                    //UserStore?.CreateUser(new UserWithRolesDto(new UserDto(result.Result.Id,$"{result.Result.FirstName} {result.Result.LastName}", result.Result.Email,
                    //    result.Result.PhoneNumber, result.Result.UserName, result.Result.Status, result.Result.CreatedAt,false,null));    
                    var Window = p as AddUserWindow;
                    Window?.Close();
                        //var role = new RoleForUserCreationDto(result.Result.Id, SelectedRole.Id);
                        //var roleResult = await AppServices.ServiceManager.UserRoleService.CreateUserRoles(role);
                        //if(roleResult is not null && roleResult is ApiOkResponse<UserRoleDto> roleResponse && roleResponse.Result is not null)
                        //{
                        //    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Utilisateur ajouté avec succès"));
                        //}else if(roleResult is not null && roleResult is ApiBadRequestResponse error)
                        //{
                        //    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de l'ajout d'un utilisateur"));
                        //}
                    
                }else if(Result is not null && Result is ApiBadRequestResponse error)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error,error.Message /*"Erreur lors de l'ajout d'un utilisateur"*/));
                }
                await Task.Delay(2000);
            });
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName); /*&& !string.IsNullOrEmpty(Email) 
                && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(Status) ;*/
        }

        private async Task GetRoles()
        {
            var roles = await AppServices.ServiceManager.RoleService.GetAllRolesAsync(false);
            if(roles is not null && roles is ApiOkResponse<IEnumerable<RoleDto>> result && result.Result is not null)
            {
                RoleDtos = new List<RoleDto>(result.Result);
            }else if(roles is not null && roles is ApiBadRequestResponse error)
            {
                RoleDtos = new List<RoleDto>();

                //await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, error.Message));
            }else if (roles is not null && roles is ApiNotFoundResponse)
            {
                RoleDtos = new List<RoleDto>();
                //await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Aucun rôle trouvé"));
            }

        }
    }
}
