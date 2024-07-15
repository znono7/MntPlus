using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRoleRepository : RepositoryBase<UserRole> , IUserRoleRepository
    {
        public UserRoleRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateUserRole(UserRole userRole)
        {
            Create(userRole);
        }

        public void DeleteUserRole(UserRole userRole)
        {
            Delete(userRole);
        }

        public async Task<IEnumerable<UserRole>?> GetAllUserRolesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

     
       

        public async Task<UserRole?> GetUserRoleAsync(Guid userRoleId, bool trackChanges)
        {
            return await FindByCondition(userRole => userRole.Id.Equals(userRoleId), trackChanges).SingleOrDefaultAsync();
        }
    }
   
}
