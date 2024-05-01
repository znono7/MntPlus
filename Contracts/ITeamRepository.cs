using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>?> GetAllTeamsAsync(bool trackChanges);
        Task<Team?> GetTeamAsync(Guid teamId, bool trackChanges);
        void CreateTeam(Team team);
        void DeleteTeam(Team team);
    }
}
