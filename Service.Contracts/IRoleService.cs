using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IRoleService
    {
        Task<ApiBaseResponse> GetAllRolesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetRoleAsync(Guid roleId, bool trackChanges);
        Task<ApiBaseResponse> CreateRole(RoleDtoForCreation role);
        Task<ApiBaseResponse> DeleteRole(Guid roleId, bool trackChanges);
    }
}
