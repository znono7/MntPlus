using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TeamMapper
    {
        public static TeamDto? Map(Team? team)
        {
            if (team == null)
            {
                return null;
            }
            return new TeamDto(team.Id, team.Name ?? "");
        }
    }
}
