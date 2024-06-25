namespace Shared
{
    public record DailyScheduleDto(Guid Id,
                                  string FrequencyType,
                                  int Interval,
                                  DateTime StartDate,
                                  DateTime? EndDate);
    public record DailyScheduleDtoForCreation(string FrequencyType,
                                 int Interval,
                                 DateTime StartDate,
                                 DateTime? EndDate);


    public record WeeklyScheduleDto(Guid Id,
                                    string FrequencyType,
                                    int Interval,
                                    DateTime StartDate,
                                    DateTime? EndDate,
                                    List<DayOfWeek> DaysOfWeek);
    public record WeeklyScheduleDtoForCreation(string FrequencyType,
                                               int Interval,
                                               DateTime StartDate,
                                               DateTime? EndDate,
                                               List<DayOfWeek> DaysOfWeek);

    public record MonthlyNumericScheduleDto(Guid Id,
                                            string FrequencyType,
                                            int Interval,
                                            DateTime StartDate,
                                            DateTime? EndDate,
                                            int DayOfMonth);
    public record MonthlyNumericScheduleDtoForCreation(string FrequencyType,
                                                       int Interval,
                                                       DateTime StartDate,
                                                       DateTime? EndDate,
                                                       int DayOfMonth);

    public record MonthlyWeekdayScheduleDto(Guid Id,
                                            string FrequencyType,
                                            int Interval,
                                            DateTime StartDate,
                                            DateTime? EndDate,
                                            int WeekOfMonth,
                                            DayOfWeek DayOfWeek);
    public record MonthlyWeekdayScheduleDtoForCreation(string FrequencyType,
                                                       int Interval,
                                                       DateTime StartDate,
                                                       DateTime? EndDate,
                                                       int WeekOfMonth,
                                                       DayOfWeek DayOfWeek);
    public record YearlyNumericScheduleDto(Guid Id,
                                           string FrequencyType,
                                           int Interval,
                                           DateTime StartDate,
                                           DateTime? EndDate,
                                           int DayOfMonth,
                                           int Month);
    public record YearlyNumericScheduleDtoForCreation(string FrequencyType,
                                                      int Interval,
                                                      DateTime StartDate,
                                                      DateTime? EndDate,
                                                      int DayOfMonth,
                                                      int Month);

    public record YearlyOrdinalScheduleDto(Guid Id,
                                           string FrequencyType,
                                           int Interval,
                                           DateTime StartDate,
                                           DateTime? EndDate,
                                           int WeekOfMonth,
                                           DayOfWeek DayOfWeek,
                                           int Month);
    public record YearlyOrdinalScheduleDtoForCreation(string FrequencyType,
                                                      int Interval,
                                                      DateTime StartDate,
                                                      DateTime? EndDate,
                                                      int WeekOfMonth,
                                                      DayOfWeek DayOfWeek,
                                                      int Month);

}
