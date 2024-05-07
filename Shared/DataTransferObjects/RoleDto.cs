using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record RoleDto(Guid Id, string Name, bool IsSeeded);
    public record RoleDtoForCreation(string Name, bool IsSeeded);


}
