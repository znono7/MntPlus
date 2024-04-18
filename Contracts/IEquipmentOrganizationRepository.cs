using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEquipmentOrganizationRepository
    {
        Task<IEnumerable<Organization>?> GetAllEquipmentOrganizationsAsync(bool trackChanges);
        Task<Organization?> GetEquipmentOrganizationAsync(Guid equipmentOrganizationId, bool trackChanges);
        void CreateEquipmentOrganization(Organization equipmentOrganization);
        void DeleteEquipmentOrganization(Organization equipmentOrganization);
    }
}
