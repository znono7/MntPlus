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
    public class RoleRepository : RepositoryBase<Role> , IRoleRepository
    {
        public RoleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateRole(Role role) => Create(role);
       

        public void DeleteRole(Role role) => Delete(role);
       

        public async Task<IEnumerable<Role>?> GetAllRolesAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

        public async Task<Role?> GetRoleAsync(Guid roleId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(roleId), trackChanges)
            .SingleOrDefaultAsync();
        
    }
   
}
