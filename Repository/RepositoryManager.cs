using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IEquipmentRepository> _equipmentRepository;
        private readonly Lazy<IAssignorRepository> _assignorRepository;
        private readonly Lazy<IEquipmentClassRepository> _equipmentClassRepository;
        private readonly Lazy<IEquipmentDepartmentRepository> _equipmentDepartmentRepository;
        private readonly Lazy<IEquipmentOrganizationRepository> _equipmentOrganizationRepository;
        private readonly Lazy<IEquipmentStatusRepository> equipmentStatusRepository;
        private readonly Lazy<IEquipmentTypeRepository> equipmentTypeRepository;






        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _equipmentRepository = new Lazy<IEquipmentRepository>(() => new EquipmentRepository(repositoryContext));
            _assignorRepository = new Lazy<IAssignorRepository>(() => new AssignorRepository(repositoryContext));
            _equipmentClassRepository = new Lazy<IEquipmentClassRepository>(() => new EquipmentClassRepository(repositoryContext));
            _equipmentDepartmentRepository = new Lazy<IEquipmentDepartmentRepository>(() => new EquipmentDepartmentRepository(repositoryContext));
            _equipmentOrganizationRepository = new Lazy<IEquipmentOrganizationRepository>(() => new EquipmentOrganizationRepository(repositoryContext));
            equipmentStatusRepository = new Lazy<IEquipmentStatusRepository>(() => new EquipmentStatusRepository(repositoryContext));
            equipmentTypeRepository = new Lazy<IEquipmentTypeRepository>(() => new EquipmentTypeRepository(repositoryContext));
        }
        public IEquipmentRepository Equipment => _equipmentRepository.Value;

        public IAssignorRepository Assignor => _assignorRepository.Value;

        public IEquipmentClassRepository EquipmentClass => _equipmentClassRepository.Value;

        public IEquipmentDepartmentRepository EquipmentDepartment => _equipmentDepartmentRepository.Value;

        public IEquipmentOrganizationRepository EquipmentOrganization => _equipmentOrganizationRepository.Value;

        public IEquipmentStatusRepository EquipmentStatus => equipmentStatusRepository.Value;

        public IEquipmentTypeRepository EquipmentType => equipmentTypeRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
        
    }

}
