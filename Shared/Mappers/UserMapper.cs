using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class UserMapper
    {
        public static UserByDto? Map(User? user)
        {
            if (user == null)
            {
                return null;
            }
            return new UserByDto(user.Id, $"{user.FirstName} {user.LastName}");
        }

        public static RolesDto? MapToRolesDto(User? user)
        {
            if (user == null)
            {
                return null;
            }
            if(user.UserRoles == null)
            {
                return new RolesDto(user.Id,
                                    $"{user.FirstName} {user.LastName}",
                                    user.Email,
                                    user.PhoneNumber,
                                    user.UserName,
                                    user.Status,
                                    user.CreatedAt,
                                    null);
            }
            return new RolesDto(
            user.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            user.PhoneNumber,
            user.UserName,
            user.Status,
            user.CreatedAt,
            user.UserRoles?.Select(ur => new RoleDto(ur.Role.Id, ur.Role.Name,ur.Role.IsSeeded)).ToList() ?? new List<RoleDto>()
        );
        }
    }
}
