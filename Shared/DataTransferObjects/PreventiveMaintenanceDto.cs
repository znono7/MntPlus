using Entities;

namespace Shared
{
    public record PreventiveMaintenanceDto(Guid Id,
                                           int? Number,
                                           string? Name,
                                           string? Description,
                                           string? FrequencyType,
                                           string? Priority,
                                           string? Type,
                                           DateTime? LastPerformed,
                                           DateTime? NextDue,
                                           DateTime? DateCreation,
                                           Guid? UserCreatedId,
                                           UserByDto? UserCreatedBy,
                                           Guid? UserAssignedToId,
                                           UserByDto? UserAssignedTo,
                                           Guid? TeamAssignedToId,
                                           TeamDto? TeamAssignedTo,
                                           Guid? CheckListId,
                                           CheckListDto? CheckList,
                                           Guid? AssetId,
                                           AssetMinimalDto? Asset,
                                           Guid? ScheduleId,
                                           ScheduleDto? Schedule,
                                           Guid? MeterScheduleId,
                                           MeterScheduleDto? MeterSchedule
                                           );
    public record PreventiveMaintenanceDtoForCreation(  
                                           int? Number,
                                           string? Name,
                                           string? Description,
                                           string? FrequencyType,
                                           string? Priority,
                                           string? Type,
                                           DateTime? LastPerformed,
                                           DateTime? NextDue,
                                           DateTime? DateCreation,
                                           Guid? UserCreatedId,
                                           Guid? UserAssignedToId,
                                           Guid? TeamAssignedToId,
                                           Guid? CheckListId,
                                           Guid? AssetId,
                                           ScheduleDtoForCreation? Schedule,
                                           MeterScheduleDtoForCreation? MeterSchedule,
                                           bool IsMeterSchedule
                                           );

}
