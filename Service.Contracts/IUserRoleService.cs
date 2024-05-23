using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserRoleService
    {
        Task<ApiBaseResponse> GetAllUserRolesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetUserRoleAsync(Guid userRoleId, bool trackChanges);
        Task<ApiBaseResponse> CreateUserRoles(RoleForUserCreationDto role);
        Task<ApiBaseResponse> DeleteUserRole(Guid userRoleId, bool trackChanges);
    }
}
