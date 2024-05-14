using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record UserTeamDto (UserWithRolesDto? User, TeamDto? Team); 
   
}
