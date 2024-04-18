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
    public class EquipmentTypeRepository : RepositoryBase<EquipmentType>, IEquipmentTypeRepository
    {
        public EquipmentTypeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateEquipmentType(EquipmentType equipmentType) => Create(equipmentType);
       

        public void DeleteEquipmentType(EquipmentType equipmentType) => Delete(equipmentType);
       

        public async Task<IEnumerable<EquipmentType>?> GetAllEquipmentTypesAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.EquipmentTypeName)
            .ToListAsync();
        

        public async Task<EquipmentType?> GetEquipmentTypeAsync(Guid equipmentTypeId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(equipmentTypeId), trackChanges)
            .SingleOrDefaultAsync();
       
    }
}
