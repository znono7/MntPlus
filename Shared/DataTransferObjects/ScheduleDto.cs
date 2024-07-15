namespace Shared
{

    public abstract record ScheduleDto
    {
        public Guid Id { get; set; }
        public string? FrequencyType { get; set; }
        public int Interval { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }
    }
    public abstract record ScheduleDtoForCreation
    {
        public string? FrequencyType { get; set; }
        public int Interval { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public record DailyScheduleDto : ScheduleDto
    {
    }

    public record DailyScheduleDtoForCreation : ScheduleDtoForCreation
    {
    }


    public record WeeklyScheduleDto : ScheduleDto
    {
        public List<DayOfWeek>? DaysOfWeek { get; set; }
    }

    public record WeeklyScheduleDtoForCreation : ScheduleDtoForCreation
    {
        public List<DayOfWeek>? DaysOfWeek { get; set; }
    }

    public record MonthlyNumericScheduleDto : ScheduleDto
    {
        public int DayOfMonth { get; set; }
    }

    public record MonthlyNumericScheduleDtoForCreation : ScheduleDtoForCreation
    {
        public int DayOfMonth { get; set; }
    }

    public record MonthlyWeekdayScheduleDto : ScheduleDto
    {
        public int WeekOfMonth { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }

    public record MonthlyWeekdayScheduleDtoForCreation : ScheduleDtoForCreation
    {
        public int WeekOfMonth { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
    public record YearlyNumericScheduleDto : ScheduleDto
    {
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
    }

    public record YearlyNumericScheduleDtoForCreation : ScheduleDtoForCreation
    {
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
    }

    public record YearlyOrdinalScheduleDto : ScheduleDto
    {
        public int WeekOfMonth { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int Month { get; set; }
    }

    public record YearlyOrdinalScheduleDtoForCreation : ScheduleDtoForCreation
    {
        public int WeekOfMonth { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int Month { get; set; }
    }

}
