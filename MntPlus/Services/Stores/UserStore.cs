using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class UserStore
    {
        public event Action<UserWithRolesDto?>? UserCreated;
        public event Action<UserDto?>? UserUpdated;
        public void CreateUser(UserWithRolesDto? user)
        {
            UserCreated?.Invoke(user);
        }

        public void UpdateUser(UserDto? user)
        {
            UserUpdated?.Invoke(user);
        }
    }
}
