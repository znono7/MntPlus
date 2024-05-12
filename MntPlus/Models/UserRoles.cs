using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class UserRoles
    {
        public int Id { get; set; }
        public string? Role { get; set; }
    }
    //generate a list of user roles
    public class UserRolesList
    {
        public List<UserRoles> UserRoles { get; set; }
        public UserRolesList()
        {
            UserRoles = new List<UserRoles>
            {
                new UserRoles { Id = 1, Role = "Ingénieur GMAO" },
                new UserRoles { Id = 2, Role = "Responsable" },
                new UserRoles { Id = 3, Role = "Demandeur" },
                //new UserRoles { Id = 4, Role = "Manager" },
                //new UserRoles { Id = 5, Role = "Supervisor" },
                //new UserRoles { Id = 6, Role = "Guest" },
            };
        }
    }
}
