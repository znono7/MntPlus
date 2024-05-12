using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class AddUserViewModel : BaseViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role => UserRoles?.Role;
        public string? Status { get; set; }
        public DateTime? CreatedAt => DateTime.Now;

        public UserRolesList? UserRolesList { get; set; }
        public UserRoles? UserRoles { get; set; }

        public bool SaveIsRunning { get; set; }

        public AddUserViewModel()
        {
            UserRolesList = new UserRolesList();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Email) 
                && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(UserName) 
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(Status) && ! (UserRoles is null);
        }
    }
}
