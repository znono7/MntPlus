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
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UserService(IRepositoryManager repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateUser(UserCreateDto user)
        {
            try
            {
                var userEntity = _mapper.Map<User>(user);
                _repository.User.CreateUser(userEntity);
                await _repository.SaveAsync();
                var userToReturn = _mapper.Map<UserDto>(userEntity);
                return new ApiOkResponse<UserDto>(userToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> DeleteUser(Guid userId, bool trackChanges)
        {
            try
            {
                var user = await _repository.User.GetUserAsync(userId, trackChanges);
                if (user is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.User.DeleteUser(user);
                await _repository.SaveAsync();
                var userToReturn = _mapper.Map<UserDto>(user);

                return new ApiOkResponse<UserDto>(userToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetAllUsersAsync(bool trackChanges)
        {
            try
            {
                var users = await _repository.User.GetAllUsersAsync(trackChanges);
                if (users is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(users);
                return new ApiOkResponse<IEnumerable<UserDto>>(usersToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetUserAsync(Guid userId, bool trackChanges)
        {
            try
            {
                var user = await _repository.User.GetUserAsync(userId, trackChanges);
                if (user is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var userToReturn = _mapper.Map<UserDto>(user);
                return new ApiOkResponse<UserDto>(userToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> GetUsersTeamAsync(Guid teamId, bool trackChanges)
        {
            try
            {
                var users = await _repository.User.GetUsersByTeamAsync(teamId, trackChanges);
                if (users is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(users);
                return new ApiOkResponse<IEnumerable<UserDto>>(usersToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> UpdateUser(Guid userId, UserCreateDto user, bool trackChanges)
        {
            try
            {
                var userEntity = await _repository.User.GetUserAsync(userId, trackChanges);
                if (userEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(user, userEntity);
                await _repository.SaveAsync();
                var userToReturn = _mapper.Map<UserDto>(userEntity);
                return new ApiOkResponse<UserDto>(userToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> UpdateUserForTeam(Guid userId, Guid teamId, bool usertrackChanges, bool teamtrackChanges)
        {
            try
            {
                var userEntity = await _repository.User.GetUserAsync(userId, usertrackChanges);
                if (userEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var team = await _repository.Team.GetTeamAsync(teamId, teamtrackChanges);
                if (team is null)
                {
                    return new ApiNotFoundResponse("");
                }
                userEntity.Id = teamId;
                await _repository.SaveAsync();
                var userToReturn = _mapper.Map<UserDto>(userEntity);
                return new ApiOkResponse<UserDto>(userToReturn);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);

               
            }
        }
    }
}
