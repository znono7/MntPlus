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
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _equipmentRepository = new Lazy<IEquipmentRepository>(() => new
                                        EquipmentRepository(repositoryContext)); 
        }
        public IEquipmentRepository Equipment => _equipmentRepository.Value;

        public void Save() => _repositoryContext.SaveChanges();
        
    }

}
