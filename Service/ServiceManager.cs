using AutoMapper;
using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {

        private readonly Lazy<ILocationService> _locationService;
        private readonly Lazy<IAssetService> assetService;

        private readonly Lazy<IEquipmentService> _equipmentService;
        private readonly Lazy<IAssignorService> _assignorService;
        private readonly Lazy<IEquipmentDepartmentService> _equipmentDepartmentService;
        private readonly Lazy<IEquipmentClassService> _equipmentClassService;
        private readonly Lazy<IEquipmentOrganizationService> _equipmentOrganizationService;
        private readonly Lazy<IEquipmentStatusService> _equipmentStatusService;
        private readonly Lazy<IEquipmentTypeService> equipmentTypeService;


        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper) 
          
        {
            _locationService = new Lazy<ILocationService>(() => new LocationService(repositoryManager, mapper));
            assetService = new Lazy<IAssetService>(() => new AssetService(repositoryManager, mapper));
            _equipmentService = new Lazy<IEquipmentService>(() => new EquipmentService(repositoryManager ));
            _assignorService = new Lazy<IAssignorService>(() => new AssignorService(repositoryManager ));
            _equipmentDepartmentService = new Lazy<IEquipmentDepartmentService>(() => new EquipmentDepartmentService(repositoryManager ));
            _equipmentClassService = new Lazy<IEquipmentClassService>(() => new EquipmentClassService(repositoryManager ));
            _equipmentOrganizationService = new Lazy<IEquipmentOrganizationService>(() => new EquipmentOrganizationService(repositoryManager ));
            _equipmentStatusService = new Lazy<IEquipmentStatusService>(() => new EquipmentStatusService(repositoryManager ));
            equipmentTypeService = new Lazy<IEquipmentTypeService>(() => new EquipmentTypeService(repositoryManager));
        }
        public IEquipmentService EquipmentService => _equipmentService.Value;

        public IAssignorService AssignorService => _assignorService.Value;

        public IEquipmentDepartmentService EquipmentDepartmentService => _equipmentDepartmentService.Value;

        public IEquipmentClassService EquipmentClassService => _equipmentClassService.Value;

        public IEquipmentOrganizationService EquipmentOrganizationService => _equipmentOrganizationService.Value;

        public IEquipmentStatusService EquipmentStatusService => _equipmentStatusService.Value;

        public IEquipmentTypeService EquipmentTypeService => equipmentTypeService.Value;

        public ILocationService LocationService => _locationService.Value;

        public IAssetService AssetService => assetService.Value;
    }
}
