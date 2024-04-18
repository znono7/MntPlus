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
    public class EquipmentDepartmentService : IEquipmentDepartmentService
    {
        private readonly IRepositoryManager _repository;


        public EquipmentDepartmentService(IRepositoryManager repository)
        {
            _repository = repository;

        }
        public async Task<ApiBaseResponse> CreateEquipmentDepartment(EquipmentDepartmentCreateDto equipmentDepartment)
        {
            try
            {
                var equipmentDepartmentEntity = new EquipmentDepartment
                {
                    DepartmentName = equipmentDepartment.DepartmentName,
                };
                _repository.EquipmentDepartment.CreateEquipmentDepartment(equipmentDepartmentEntity);
                await _repository.SaveAsync();
                var equipmentDepartmentToReturn = new EquipmentDepartmentDto
                (
                                       Id: equipmentDepartmentEntity.Id,
                                                          DepartmentName: equipmentDepartmentEntity.DepartmentName
                                                                         );
                return new ApiOkResponse<EquipmentDepartmentDto>(equipmentDepartmentToReturn);
            }
            catch (Exception ex)
            {

                return new AssignorCreateErrorResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> DeleteEquipmentDepartment(Guid equipmentDepartmentId, bool trackChanges)
        {
            try
            {
                var equipmentDepartment = await _repository.EquipmentDepartment.GetEquipmentDepartmentAsync(equipmentDepartmentId, trackChanges);
                if (equipmentDepartment is null)
                {
                    return new AssignorNotFoundResponse("equipment department");
                }
                _repository.EquipmentDepartment.DeleteEquipmentDepartment(equipmentDepartment);
                await _repository.SaveAsync();
                return new ApiOkResponse<EquipmentDepartment>(equipmentDepartment);

            }
            catch (Exception)
            {
                return new AssignorDeleteErrorResponse("");

                
            }
        }

        public async Task<ApiBaseResponse> GetAllEquipmentDepartmentsAsync(bool trackChanges)
        {
            try
            {
                var equipmentDepartments = await _repository.EquipmentDepartment.GetAllEquipmentDepartmentsAsync(trackChanges);
                if (equipmentDepartments is null)
                {
                    return new AssignorNotFoundResponse("equipment department");
                }
                return new ApiOkResponse<IEnumerable<EquipmentDepartment>>(equipmentDepartments);

            }
            catch (Exception ex)
            {
                return new AssignorGetListErrorResponse(ex.Message);

                throw;
            }
            
        }

        public async Task<ApiBaseResponse> GetEquipmentDepartmentAsync(Guid equipmentDepartmentId, bool trackChanges)
        {
            try
            {
                var equipmentDepartment = await _repository.EquipmentDepartment.GetEquipmentDepartmentAsync(equipmentDepartmentId, trackChanges);
                if (equipmentDepartment is null)
                {
                    return new AssignorNotFoundResponse("equipment department");
                }
                return new ApiOkResponse<EquipmentDepartment>(equipmentDepartment);

            }
            catch (Exception)
            {
                return new AssignorGetListErrorResponse("");

                
            }
        }
    }
}
