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
    public class TeamService : ITeamService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public TeamService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApiBaseResponse> CreateTeam(TeamCreatedDto team)
        {
            try
            {
                var teamEntity = new Team() { Name = team.Name};
                _repository.Team.CreateTeam(teamEntity);
                await _repository.SaveAsync();
                var teamToReturn = new TeamDto( Id : teamEntity.Id, Name : teamEntity.Name );
                return new ApiOkResponse<TeamDto>(teamToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }

           
        }

        public async Task<ApiBaseResponse> DeleteTeam(Guid teamId, bool trackChanges)
        {
            try
            {
                var team = await _repository.Team.GetTeamAsync(teamId, trackChanges);
                if (team is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _repository.Team.DeleteTeam(team);
                await _repository.SaveAsync();
                var teamToReturn = _mapper.Map<TeamDto>(team);

                return new ApiOkResponse<TeamDto>(teamToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
           
        }

        public async Task<ApiBaseResponse> GetAllTeamsAsync(bool trackChanges)
        {
            try
            {
                var teams = await _repository.Team.GetAllTeamsAsync(trackChanges);
                if (teams is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var teamsDto = teams.Select(t => new TeamDto(t.Id,t.Name!));
                return new ApiOkResponse<IEnumerable<TeamDto>>(teamsDto);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> GetTeamAsync(Guid teamId, bool trackChanges)
        {
            try
            {
                var team = await _repository.Team.GetTeamAsync(teamId, trackChanges);
                if (team is null)
                {
                    return new ApiNotFoundResponse("");
                }
                var teamToReturn = _mapper.Map<TeamDto>(team);
                return new ApiOkResponse<TeamDto>(teamToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
            
        }

        public async Task<ApiBaseResponse> UpdateTeam(Guid teamId, TeamCreatedDto team, bool trackChanges)
        {
            try
            {
                var teamEntity = await _repository.Team.GetTeamAsync(teamId, trackChanges);
                if (teamEntity is null)
                {
                    return new ApiNotFoundResponse("");
                }
                _mapper.Map(team, teamEntity);
                await _repository.SaveAsync();
                var teamToReturn = _mapper.Map<TeamDto>(teamEntity);
                return new ApiOkResponse<TeamDto>(teamToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
           
        }
    }
}
