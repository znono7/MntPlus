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

        public IEnumerable<Equipment> GetAllEquipments(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.EquipmentName)
            .ToList();
        
    }
}
