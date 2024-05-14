using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class UserTeamStore
    {
        public event Action<UserTeamDto?>? UserTeamCreated;
        public event Action<UserTeamDto?>? UserTeamUpdated;
        public event Action<UserTeamDto?>? UserTeamSelected;
        public void CreateUserTeam(UserTeamDto? userTeam)
        {
            UserTeamCreated?.Invoke(userTeam);
        }

        public void UpdateUserTeam(UserTeamDto? userTeam)
        {
            UserTeamUpdated?.Invoke(userTeam);
        }

        public void SelectUserTeam(UserTeamDto? userTeam)
        {
            UserTeamSelected?.Invoke(userTeam);
        }
    }
}
