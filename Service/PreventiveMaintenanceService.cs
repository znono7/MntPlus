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
    public class PreventiveMaintenanceService : IPreventiveMaintenanceService
    {
        private readonly IRepositoryManager _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PreventiveMaintenanceService(IRepositoryManager repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiBaseResponse> BulkDeletePreventiveMaintenance(List<Guid?> preventiveMaintenanceIds, bool trackChanges)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var preventiveMaintenance = await _repository.PreventiveMaintenance.GetPreventiveMaintenancesByIdsAsync(preventiveMaintenanceIds, trackChanges);
                if (preventiveMaintenance == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiNotFoundResponse("Maintenance préventive non trouvée");
                }
                _repository.PreventiveMaintenance.BulkDeletePreventiveMaintenance(preventiveMaintenance);
                await _unitOfWork.SaveChangesAsync();
                List<Schedule> schedules = preventiveMaintenance.Where(pm => pm.ScheduleId != null)
                    .Select(pm => pm.Schedule!).ToList();
                
                var meterSchedule = preventiveMaintenance.Where(pm => pm.MeterScheduleId != null).Select(pm => pm.MeterSchedule!).ToList();  
                if(schedules != null && schedules.Count > 0)
                {
                    _repository.Schedule.DeleteSchedules(schedules);
                    await _unitOfWork.SaveChangesAsync();
                }
                if(meterSchedule != null && meterSchedule.Count > 0)
                {
                    _repository.MeterSchedule.DeleteMeterSchedules(meterSchedule);
                    await _unitOfWork.SaveChangesAsync();
                }
                await _unitOfWork.CommitTransactionAsync();
                return new ApiOkResponse<string>("Maintenance préventive supprimée avec succès");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> CreateLastNumberPreventiveMaintenance()
        {
            try
            {
                var Num = await _repository.PreventiveMaintenance.GetNextPreventiveMaintenanceNumberAsync();
                
                return new ApiOkResponse<int?>(Num);

            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public async Task<ApiBaseResponse> CreatePreventiveMaintenance(PreventiveMaintenanceDtoForCreation preventiveMaintenance)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                
                if(preventiveMaintenance.Schedule == null && preventiveMaintenance.MeterSchedule == null)
                {
                    return new ApiPropertyNullRequestResponse("Un horaire ou un horaire de compteur est requis");
                }
                PreventiveMaintenance preventiveMaintenanceEntity = PreventiveMaintenanceMapper.Map(preventiveMaintenance);
                Schedule? schedule = null;
                MeterSchedule? meterSchedule = null;
                if (preventiveMaintenance.Schedule != null)
                {
                    schedule = ScheduleMapper.Map(preventiveMaintenance.Schedule);
                    _repository.Schedule.CreateSchedule(schedule);
                    await _unitOfWork.SaveChangesAsync();
                    preventiveMaintenanceEntity.ScheduleId = schedule.Id;
                }
                if (preventiveMaintenance.MeterSchedule != null)
                {
                    meterSchedule = MeterScheduleMapper.Map(preventiveMaintenance.MeterSchedule);
                    _repository.MeterSchedule.CreateMeterSchedule(meterSchedule);
                    await _unitOfWork.SaveChangesAsync();
                    preventiveMaintenanceEntity.MeterScheduleId = meterSchedule.Id;
                }
               
                _repository.PreventiveMaintenance.CreatePreventiveMaintenance(preventiveMaintenanceEntity);
                await _unitOfWork.SaveChangesAsync();
                var preventiveMaintenanceDb = await _repository.PreventiveMaintenance.GetPreventiveMaintenanceAsync(preventiveMaintenanceEntity.Id, false);
                if (preventiveMaintenanceDb == null) 
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return new ApiBadRequestResponse("");
                }
                await _unitOfWork.CommitTransactionAsync();
                var preventiveMaintenanceToReturn = PreventiveMaintenanceMapper.Map(preventiveMaintenanceDb);
                return new ApiOkResponse<PreventiveMaintenanceDto>(preventiveMaintenanceToReturn);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return new ApiBadRequestResponse(ex.Message);

            }
        }

        public Task<ApiBaseResponse> DeletePreventiveMaintenance(Guid preventiveMaintenanceId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiBaseResponse> GetAllPreventiveMaintenancesAsync(bool trackChanges)
        {
            try
            {
                var preventiveMaintenances = await _repository.PreventiveMaintenance.GetAllPreventiveMaintenancesAsync(trackChanges);
                if(preventiveMaintenances == null)
                {
                    return new ApiNotFoundResponse("Aucune maintenance préventive trouvée");
                }
                var preventiveMaintenancesToReturn = preventiveMaintenances.Select(pm => PreventiveMaintenanceMapper.Map(pm));
                return new ApiOkResponse<IEnumerable<PreventiveMaintenanceDto>>(preventiveMaintenancesToReturn);
            }
            catch (Exception ex)
            {
                return new ApiBadRequestResponse(ex.Message);
            }
        }

        public Task<ApiBaseResponse> GetPreventiveMaintenanceAsync(Guid preventiveMaintenanceId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<ApiBaseResponse> UpdatePreventiveMaintenance(Guid preventiveMaintenanceId, PreventiveMaintenanceDtoForCreation preventiveMaintenance, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<ApiBaseResponse> UpdateStatPreventiveMaintenance(Guid preventiveMaintenanceId, string preventiveMaintenanceStat, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
