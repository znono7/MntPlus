using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEquipmentOrganizationService
    {


        Task<ApiBaseResponse> GetEquipmentOrganizationsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetEquipmentOrganizationAsync(Guid OrganizationId, bool trackChanges);
        Task<ApiBaseResponse> CreateEquipmentOrganizationAsync(EquipmentOrganizationCreateDto equipmentOrganization);
        Task<ApiBaseResponse> DeleteEquipmentOrganizationAsync(Guid OrganizationId, bool trackChanges);

    }
}
