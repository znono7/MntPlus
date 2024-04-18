using Contracts;
using Entities;
using Service.Contracts;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EquipmentOrganizationService : IEquipmentOrganizationService
    {
        private readonly IRepositoryManager _repository;


        public EquipmentOrganizationService(IRepositoryManager repository)
        {
            _repository = repository;

        }

        public async Task<ApiBaseResponse> CreateEquipmentOrganizationAsync(EquipmentOrganizationCreateDto equipmentOrganization)
        {
            try
            {
                var equipmentOrganizationEntity = new Organization
                {
                    OrganizationName = equipmentOrganization.OrganizationName,
                };
                _repository.EquipmentOrganization.CreateEquipmentOrganization(equipmentOrganizationEntity);
                await _repository.SaveAsync();
                var equipmentOrganizationToReturn = new EquipmentOrganizationDto
                (
                                       Id: equipmentOrganizationEntity.Id,
                                                          OrganizationName: equipmentOrganizationEntity.OrganizationName
                                                                                            );
                return new ApiOkResponse<EquipmentOrganizationDto>(equipmentOrganizationToReturn);
            }
            catch (Exception ex)
            {

                return new AssignorCreateErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteEquipmentOrganizationAsync(Guid OrganizationId, bool trackChanges)
        {
            try
            {
                var equipmentOrganization = await _repository.EquipmentOrganization.GetEquipmentOrganizationAsync(OrganizationId, trackChanges);
                if (equipmentOrganization is null)
                {
                    return new AssignorNotFoundResponse("assignor");
                }
                _repository.EquipmentOrganization.DeleteEquipmentOrganization(equipmentOrganization);
                await _repository.SaveAsync();
                return new ApiOkResponse<Organization>(equipmentOrganization);
            }
            catch (Exception ex)
            {

                return new AssignorDeleteErrorResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetEquipmentOrganizationAsync(Guid OrganizationId, bool trackChanges)
        {
            try
            {
                var equipmentOrganization = await _repository.EquipmentOrganization.GetEquipmentOrganizationAsync(OrganizationId, trackChanges);
                if (equipmentOrganization is null)
                {
                    return new AssignorNotFoundResponse("Organization");
                }
                return new ApiOkResponse<Organization>(equipmentOrganization);
            }
            catch (Exception ex)
            {

                return new AssignorGetListErrorResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetEquipmentOrganizationsAsync(bool trackChanges)
        {
            try
            {
                var equipmentOrganizations = await _repository.EquipmentOrganization.GetAllEquipmentOrganizationsAsync(trackChanges);
                if (equipmentOrganizations is null)
                {

                    return new AssignorNotFoundResponse("Organization");
                }
                var equipmentOrganizationsDto = equipmentOrganizations.Select(item => new EquipmentOrganizationDto(item.Id, item.OrganizationName)).ToList();

                return new ApiOkResponse<IEnumerable<EquipmentOrganizationDto>>(equipmentOrganizationsDto);
            }
            catch (Exception ex)
            {

                return new AssignorGetListErrorResponse(ex.Message);
            }
        }
    }
}
