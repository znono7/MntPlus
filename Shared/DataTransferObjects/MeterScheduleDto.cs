namespace Shared
{
    public record MeterScheduleDto(Guid Id,
                                   string? FrequencyType,
                                   int Interval,
                                   DateTime? StartDate,
                                   DateTime? EndDate,
                                   MeterDto? Meter);
    public record MeterScheduleDtoForCreation(string FrequencyType,
                                              int Interval,
                                               DateTime? StartDate,
                                               DateTime? EndDate,
                                              Guid MeterId); 


}
