using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserService 
    {
        Task<ApiBaseResponse> GetAllUsersAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAllUsersWithRolesAsync(bool trackChanges);
        Task<ApiBaseResponse> GetUserAsync(Guid userId, bool trackChanges);
        Task<ApiBaseResponse> GetUsersTeamAsync(Guid teamId, bool trackChanges);
        Task<ApiBaseResponse> CreateUser(UserCreateDto user);
        Task<ApiBaseResponse> DeleteUser(Guid userId, bool trackChanges);
        Task<ApiBaseResponse> UpdateUser(Guid userId, UserCreateDto user, bool trackChanges);
        Task<ApiBaseResponse> UpdateUserForTeam(Guid userId, Guid teamId , bool usertrackChanges , bool teamtrackChanges);

    }
}
