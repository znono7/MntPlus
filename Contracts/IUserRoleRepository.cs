using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>?> GetAllUserRolesAsync(bool trackChanges);
        Task<UserRole?> GetUserRoleAsync(Guid userRoleId, bool trackChanges);
        void CreateUserRole(UserRole userRole);
        void DeleteUserRole(UserRole userRole);
    }
}
