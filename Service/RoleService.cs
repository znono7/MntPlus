using AutoMapper;
using Contracts;
using Entities;
using Service.Contracts;
using Shared;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public RoleService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> CreateRole(RoleDtoForCreation role)
        {
            try
            {
                var roleEntity = _mapper.Map<Role>(role);
                _repository.Role.CreateRole(roleEntity);
                await _repository.SaveAsync();
                var roleToReturn = _mapper.Map<RoleDto>(roleEntity);
                return new ApiOkResponse<RoleDto>(roleToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

        }
       
        public async Task<ApiBaseResponse> DeleteRole(Guid roleId, bool trackChanges)
        {
            try
            {
                var role = await _repository.Role.GetRoleAsync(roleId, trackChanges);
                if (role is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Role.DeleteRole(role);
                await _repository.SaveAsync();
                var roleToReturn = _mapper.Map<RoleDto>(role);

                return new ApiOkResponse<RoleDto>(roleToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> GetAllRolesAsync(bool trackChanges)
        {
            try
            {
                var roles = await _repository.Role.GetAllRolesAsync(trackChanges);
                var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(roles);
                return new ApiOkResponse<IEnumerable<RoleDto>>(rolesDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetRoleAsync(Guid roleId, bool trackChanges)
        {
            try
            {
                var role = await _repository.Role.GetRoleAsync(roleId, trackChanges);
                if (role is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var roleDto = _mapper.Map<RoleDto>(role);
                return new ApiOkResponse<RoleDto>(roleDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

      
    }
}
