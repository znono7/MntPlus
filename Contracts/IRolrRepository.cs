using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>?> GetAllRolesAsync(bool trackChanges);
        Task<Role?> GetRoleAsync(Guid roleId, bool trackChanges);
        void CreateRole(Role role);
        void DeleteRole(Role role);
    }
}
