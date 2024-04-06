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
        private readonly Lazy<IEquipmentService> _equipmentService;

        public ServiceManager(IRepositoryManager repositoryManager , ILoggerManager logger) 
          
        {
            _equipmentService = new Lazy<IEquipmentService>(() => new EquipmentService(repositoryManager , logger));
        }
        public IEquipmentService EquipmentService => _equipmentService.Value;
    }
}
