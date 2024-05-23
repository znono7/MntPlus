using AutoMapper;
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
    public class UserRoleService : IUserRoleService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UserRoleService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateUserRoles(RoleForUserCreationDto role)
        {
            try
            {
                var roleEntity = _mapper.Map<UserRole>(role);
                _repository.UserRole.CreateUserRole(roleEntity); 
                await _repository.SaveAsync();
                var roleToReturn = _mapper.Map<UserRoleDto>(roleEntity);
                return new ApiOkResponse<UserRoleDto>(roleToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteUserRole(Guid userRoleId, bool trackChanges)
        {
            try
            {
                var role = await _repository.UserRole.GetUserRoleAsync(userRoleId, trackChanges);
                if (role is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.UserRole.DeleteUserRole(role);
                await _repository.SaveAsync();
                var roleToReturn = _mapper.Map<UserRoleDto>(role);
                return new ApiOkResponse<UserRoleDto>(roleToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
               
            }

        }

        public Task<ApiBaseResponse> GetAllUserRolesAsync(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<ApiBaseResponse> GetUserRoleAsync(Guid userRoleId, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
