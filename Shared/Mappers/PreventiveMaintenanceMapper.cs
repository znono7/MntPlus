using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PreventiveMaintenanceMapper
    {
        public static PreventiveMaintenance Map(PreventiveMaintenanceDtoForCreation preventiveMaintenanceDto)
        {


            return new PreventiveMaintenance
            {
                Number = preventiveMaintenanceDto.Number,
                Name = preventiveMaintenanceDto.Name,
                Description = preventiveMaintenanceDto.Description,
                FrequencyType = preventiveMaintenanceDto.FrequencyType,
                Priority = preventiveMaintenanceDto.Priority,
                Type = preventiveMaintenanceDto.Type,
                LastPerformed = preventiveMaintenanceDto.LastPerformed,
                NextDue = preventiveMaintenanceDto.NextDue,
                DateCreation = preventiveMaintenanceDto.DateCreation,
                UserCreatedId = preventiveMaintenanceDto.UserCreatedId,
                UserAssignedToId = preventiveMaintenanceDto.UserAssignedToId,
                TeamAssignedToId = preventiveMaintenanceDto.TeamAssignedToId,
                CheckListId = preventiveMaintenanceDto.CheckListId,
                AssetId = preventiveMaintenanceDto.AssetId,
               
            };
        }

        public static PreventiveMaintenanceDto Map(PreventiveMaintenance preventiveMaintenance)
        {
            return new PreventiveMaintenanceDto(preventiveMaintenance.Id,
                                                preventiveMaintenance.Number, 
                                                preventiveMaintenance.Name,
                                                preventiveMaintenance.Description,
                                                preventiveMaintenance.FrequencyType,
                                                preventiveMaintenance.Priority,
                                                preventiveMaintenance.Type,
                                                preventiveMaintenance.LastPerformed,
                                                preventiveMaintenance.NextDue,
                                                preventiveMaintenance.DateCreation,
                                                preventiveMaintenance.UserCreatedId,
                                                UserMapper.Map(preventiveMaintenance.UserCreatedBy),
                                                preventiveMaintenance.UserAssignedToId,
                                                UserMapper.Map(preventiveMaintenance.UserAssignedTo),
                                                preventiveMaintenance.TeamAssignedToId,
                                                TeamMapper.Map(preventiveMaintenance.TeamAssignedTo),
                                                preventiveMaintenance.CheckListId,
                                                CheckListMapper.Map(preventiveMaintenance.CheckList),
                                                preventiveMaintenance.AssetId,
                                                AssetMapper.Map(preventiveMaintenance.Asset),
                                                preventiveMaintenance.ScheduleId,
                                                ScheduleMapper.Map(preventiveMaintenance.Schedule),
                                                preventiveMaintenance.MeterScheduleId,
                                                MeterScheduleMapper.Map(preventiveMaintenance.MeterSchedule));
        }
    }
}
