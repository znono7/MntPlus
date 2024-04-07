using Contracts;
using Entities;
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
       

        public IEnumerable<Equipment> GetAllEquipments(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.EquipmentName)
            .ToList();

        public Equipment? GetEquipment(Guid equipmentId, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(equipmentId), trackChanges)
            .SingleOrDefault();
       
    }
}
