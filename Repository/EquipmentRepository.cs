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
    public class EquipmentRepository : RepositoryBase<Equipment> , IEquipmentRepository
    {
        public EquipmentRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateEquipment(Equipment equipment) => Create(equipment);
       

        public async Task<IEnumerable<Equipment>> GetAllEquipmentsAsync(bool trackChanges) =>
           await FindAll(trackChanges)
            .OrderBy(c => c.EquipmentName)
            .ToListAsync();

        public async Task<Equipment?> GetEquipmentAsync(Guid equipmentId, bool trackChanges) =>
           await FindByCondition(c => c.Id.Equals(equipmentId), trackChanges)
            .SingleOrDefaultAsync();
       
    }
}
