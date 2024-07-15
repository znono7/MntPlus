using Entities;

namespace Shared
{
    public class ScheduleMapper
    {
        public static ScheduleDto? Map(Schedule? schedule)
        {
            if (schedule == null)
            {
                return null;
            }
            return schedule switch
            {
                DailySchedule ds => new DailyScheduleDto
                {
                    Id = ds.Id,
                    FrequencyType = ds.FrequencyType,
                    Interval = ds.Interval,
                    StartDate = ds.StartDate,
                    EndDate = ds.EndDate
                },
                WeeklySchedule ws => new WeeklyScheduleDto
                {
                    Id = ws.Id,
                    FrequencyType = ws.FrequencyType,
                    Interval = ws.Interval,
                    StartDate = ws.StartDate,
                    EndDate = ws.EndDate,
                    DaysOfWeek = ws.DaysOfWeek
                },
                MonthlyNumericSchedule mns => new MonthlyNumericScheduleDto
                {
                    Id = mns.Id,
                    FrequencyType = mns.FrequencyType,
                    Interval = mns.Interval,
                    StartDate = mns.StartDate,
                    EndDate = mns.EndDate,
                    DayOfMonth = mns.DayOfMonth
                },
                MonthlyWeekdaySchedule mws => new MonthlyWeekdayScheduleDto
                {
                    Id = mws.Id,
                    FrequencyType = mws.FrequencyType,
                    Interval = mws.Interval,
                    StartDate = mws.StartDate,
                    EndDate = mws.EndDate,
                    WeekOfMonth = mws.WeekOfMonth,
                    DayOfWeek = mws.DayOfWeek
                },
                YearlyNumericSchedule yns => new YearlyNumericScheduleDto
                {
                    Id = yns.Id,
                    FrequencyType = yns.FrequencyType,
                    Interval = yns.Interval,
                    StartDate = yns.StartDate,
                    EndDate = yns.EndDate,
                    DayOfMonth = yns.DayOfMonth,
                    Month = yns.Month
                },
                YearlyOrdinalSchedule yos => new YearlyOrdinalScheduleDto
                {
                    Id = yos.Id,
                    FrequencyType = yos.FrequencyType,
                    Interval = yos.Interval,
                    StartDate = yos.StartDate,
                    EndDate = yos.EndDate,
                    WeekOfMonth = yos.WeekOfMonth,
                    DayOfWeek = yos.DayOfWeek,
                    Month = yos.Month
                },
                _ => throw new NotSupportedException("Unknown schedule type")
            };
        }

        public static Schedule Map(ScheduleDtoForCreation schedule)
        {
            return schedule switch
            {
                DailyScheduleDtoForCreation ds => new DailySchedule
                {
                    FrequencyType = ds.FrequencyType,
                    Interval = ds.Interval,
                    StartDate = ds.StartDate,
                    EndDate = ds.EndDate
                },
                WeeklyScheduleDtoForCreation ws => new WeeklySchedule
                {
                    FrequencyType = ws.FrequencyType,
                    Interval = ws.Interval,
                    StartDate = ws.StartDate,
                    EndDate = ws.EndDate,
                    DaysOfWeek = ws.DaysOfWeek
                },
                MonthlyNumericScheduleDtoForCreation mns => new MonthlyNumericSchedule
                {
                    FrequencyType = mns.FrequencyType,
                    Interval = mns.Interval,
                    StartDate = mns.StartDate,
                    EndDate = mns.EndDate,
                    DayOfMonth = mns.DayOfMonth
                },
                MonthlyWeekdayScheduleDtoForCreation mws => new MonthlyWeekdaySchedule
                {
                    FrequencyType = mws.FrequencyType,
                    Interval = mws.Interval,
                    StartDate = mws.StartDate,
                    EndDate = mws.EndDate,
                    WeekOfMonth = mws.WeekOfMonth,
                    DayOfWeek = mws.DayOfWeek
                },
                YearlyNumericScheduleDtoForCreation yns => new YearlyNumericSchedule
                {
                    FrequencyType = yns.FrequencyType,
                    Interval = yns.Interval,
                    StartDate = yns.StartDate,
                    EndDate = yns.EndDate,
                    DayOfMonth = yns.DayOfMonth,
                    Month = yns.Month
                },
                YearlyOrdinalScheduleDtoForCreation yos => new YearlyOrdinalSchedule
                {
                    FrequencyType = yos.FrequencyType,
                    Interval = yos.Interval,
                    StartDate = yos.StartDate,
                    EndDate = yos.EndDate,
                    WeekOfMonth = yos.WeekOfMonth,
                    DayOfWeek = yos.DayOfWeek,
                    Month = yos.Month
                },
                _ => throw new NotSupportedException("Unknown schedule type")
            };
        }

    }

}
