namespace Shared
{
    public record MeterScheduleDto(Guid Id, string FrequencyType, int Interval);
    public record MeterScheduleDtoForCreation( string FrequencyType, int Interval , Guid MeterId);


}
