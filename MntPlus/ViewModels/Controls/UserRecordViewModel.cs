using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class UserRecordViewModel : BaseViewModel
    {
        public string ProfilePictureRGB { get; set; } = "FF0000";
        public string? Initials { get; set; }
        public ColorsList ColorsList { get; set; }
        public string? FullName { get; set; }    
        public string? Email { get; set; }
        public RolesDto? RolesDto { get; set; }
        public ObservableCollection<RoleTageViewModel> RoleTages { get; set; } 
        public UserRecordViewModel(RolesDto? rolesDto)
        {
            RolesDto = rolesDto;
            if (rolesDto != null)
            {
                Initials = GetInitials(rolesDto.FullName!);
                FullName = rolesDto.FullName;
                Email = rolesDto.Email;
                if(rolesDto.Roles != null)
                {
                    RoleTages = new ObservableCollection<RoleTageViewModel>();
                    foreach (var role in rolesDto.Roles)
                    {
                        RoleTages.Add(new RoleTageViewModel(role));
                    }
                }
            }

            ColorsList = new ColorsList();
            AssignRandomColorsFromList();
        }
        public static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return string.Empty;
            }

            string[] names = fullName.Split(' ');
            string initials = string.Empty;

            foreach (string name in names)
            {
                if (!string.IsNullOrWhiteSpace(name) && name.Length > 0)
                {
                    initials += name[0];
                }
            }

            return initials.ToUpper();
        }
        private void AssignRandomColorsFromList()
        {
            Random random = new Random();
            int colorCount = ColorsList.Colors.Count;
            ProfilePictureRGB = ColorsList.Colors[random.Next(colorCount)];
           
        }
    }

    public class RoleTageViewModel : BaseViewModel
    {
        public string RoleTageRGB { get; set; } = "FF0000";
        public string? RoleName { get; set; }
        public RoleDto? RoleDto { get; }

        public RoleTageViewModel(RoleDto? roleDto)
        {
            RoleDto = roleDto;
            if (roleDto != null)
            {
                RoleName = roleDto.Name;
                RoleTageRGB = SetRoleTageRGB(roleDto.Name);
            }
        }

        private string SetRoleTageRGB(string roleName)
        {
            switch (roleName)
            {
                case "Ingénieur GMAO":
                    return "1C9151";
                case "Responsable":
                    return "003032";
                case "Demandeur":
                    return "FBCE60";
                default:
                    return "0000FF";
            }
        }
    }
}
