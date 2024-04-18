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
    public class EquipmentDepartmentRepository : RepositoryBase<EquipmentDepartment>, IEquipmentDepartmentRepository
    {
        public EquipmentDepartmentRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }
        public void CreateEquipmentDepartment(EquipmentDepartment equipmentDepartment) => Create(equipmentDepartment);
       

        public void DeleteEquipmentDepartment(EquipmentDepartment equipmentDepartment)  
            => Delete(equipmentDepartment);
       

        public async Task<IEnumerable<EquipmentDepartment>?> GetAllEquipmentDepartmentsAsync(bool trackChanges) 
            => await FindAll(trackChanges)
                .OrderBy(c => c.DepartmentName)
                .ToListAsync();
      

        public async Task<EquipmentDepartment?> GetEquipmentDepartmentAsync(Guid equipmentDepartmentId, bool trackChanges) 
            => await FindByCondition(c => c.Id.Equals(equipmentDepartmentId), trackChanges)
                .SingleOrDefaultAsync();
      
    }
}
