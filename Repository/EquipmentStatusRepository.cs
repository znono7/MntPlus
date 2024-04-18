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
    public class EquipmentStatusRepository : RepositoryBase<EquipmentStatus>, IEquipmentStatusRepository
    {
        public EquipmentStatusRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateEquipmentStatus(EquipmentStatus equipmentStatus) => Create(equipmentStatus);
      

        public void DeleteEquipmentStatus(EquipmentStatus equipmentStatus) => Delete(equipmentStatus);
        

        public async Task<IEnumerable<EquipmentStatus>?> GetAllEquipmentStatusAsync(bool trackChanges) =>
             await FindAll(trackChanges)
            .OrderBy(c => c.EquipmentStatusName)
            .ToListAsync();
       

        public async Task<EquipmentStatus?> GetEquipmentStatusByIdAsync(Guid equipmentStatusId, bool trackChanges) =>
            await FindByCondition(c => c.Id.Equals(equipmentStatusId), trackChanges)
            .SingleOrDefaultAsync();
        
    }
}
