using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record UserRoleDto(Guid Id, Guid? UserId,UserDto? User,RoleDto? Role, Guid? RoleId);
    public record RoleForUserCreationDto (Guid UserId , Guid RoleId); 

    public record RolesDto(
        Guid Id,
        string? FullName,
        string? Email,
        string? PhoneNumber,
        string? UserName,
        string? Status,
        DateTime? CreatedAt,
        List<RoleDto>? Roles);
    
}
 