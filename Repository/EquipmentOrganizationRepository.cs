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
    public class EquipmentOrganizationRepository : RepositoryBase<Organization>, IEquipmentOrganizationRepository
    {
        public EquipmentOrganizationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateEquipmentOrganization(Organization equipmentOrganization) => Create(equipmentOrganization);
       

        public void DeleteEquipmentOrganization(Organization equipmentOrganization) => Delete(equipmentOrganization);
     

        public async Task<IEnumerable<Organization>?> GetAllEquipmentOrganizationsAsync(bool trackChanges) => 
            await FindAll(trackChanges).OrderBy(e => e.OrganizationName).ToListAsync();
       

        public async Task<Organization?> GetEquipmentOrganizationAsync(Guid equipmentOrganizationId, bool trackChanges) =>
           await FindByCondition(e => e.Id.Equals(equipmentOrganizationId), trackChanges).SingleOrDefaultAsync();
        
    }
}
