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
    public class EquipmentClassRepository : RepositoryBase<EquipmentClass>, IEquipmentClassRepository
    {
        
        public EquipmentClassRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateEquipmentClass(EquipmentClass equipmentClass) => Create(equipmentClass);
        

        public void DeleteEquipmentClass(EquipmentClass equipmentClass) => Delete(equipmentClass);
      

        public async Task<IEnumerable<EquipmentClass>?> GetAllEquipmentClassesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(c => c.EquipmentClassName)
                .ToListAsync();
        }

            

        public async Task<EquipmentClass?> GetEquipmentClassAsync(Guid equipmentClassId, bool trackChanges)
        
            => await FindByCondition(c => c.Id.Equals(equipmentClassId), trackChanges)
                .SingleOrDefaultAsync();
        
       
    }
}
