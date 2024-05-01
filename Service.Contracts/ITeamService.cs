using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ITeamService
    {
        Task<ApiBaseResponse> GetAllTeamsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetTeamAsync(Guid teamId, bool trackChanges);
        Task<ApiBaseResponse> CreateTeam(TeamCreatedDto team);
        Task<ApiBaseResponse> DeleteTeam(Guid teamId, bool trackChanges);
        Task<ApiBaseResponse> UpdateTeam(Guid teamId, TeamCreatedDto team, bool trackChanges);

    }
}
