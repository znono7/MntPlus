using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public static class ScheduleCalculator
    { 
        public static DateTime GetNextDueDate(ScheduleDto? schedule, DateTime fromDate)
        {
            switch (schedule) 
            {
                case DailyScheduleDto DailyScheduleDto:
                    return GetNextDueDateForDaily(DailyScheduleDto, fromDate);

                case WeeklyScheduleDto WeeklyScheduleDto:
                    return GetNextDueDateForWeekly(WeeklyScheduleDto, fromDate);

                case MonthlyNumericScheduleDto MonthlyNumericScheduleDto:
                    return GetNextDueDateForMonthlyNumeric(MonthlyNumericScheduleDto, fromDate);

                case MonthlyWeekdayScheduleDto MonthlyWeekdayScheduleDto:
                    return GetNextDueDateForMonthlyWeekday(MonthlyWeekdayScheduleDto, fromDate);

                case YearlyNumericScheduleDto YearlyNumericScheduleDto:
                    return GetNextDueDateForYearlyNumeric(YearlyNumericScheduleDto, fromDate);

                case YearlyOrdinalScheduleDto YearlyOrdinalScheduleDto:
                    return GetNextDueDateForYearlyOrdinal(YearlyOrdinalScheduleDto, fromDate);

                default:
                    throw new NotSupportedException("Unknown schedule type");
            }
        }

        private static DateTime GetNextDueDateForDaily(DailyScheduleDto schedule, DateTime fromDate)
        {
            var nextDueDate = fromDate.AddDays(schedule.Interval);
            if(schedule.EndDate != null)
            {
                return nextDueDate<= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }else
            {
                return nextDueDate;
            }
        }

        private static DateTime GetNextDueDateForWeekly(WeeklyScheduleDto schedule, DateTime fromDate)
        {
            var nextDueDate = fromDate.AddDays(schedule.Interval * 7);
            var dayOffset = schedule.DaysOfWeek!.Select(day => ((int)day - (int)nextDueDate.DayOfWeek + 7) % 7).Min() ;
            nextDueDate = nextDueDate.AddDays(dayOffset);
            if (schedule.EndDate != null)
            {
                return nextDueDate <= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }
            else
            {
                return nextDueDate;
            }
        }

        private static DateTime GetNextDueDateForMonthlyNumeric(MonthlyNumericScheduleDto schedule, DateTime fromDate)
        {
            var nextDueDate = fromDate.AddMonths(schedule.Interval);
            if (schedule.DayOfMonth > DateTime.DaysInMonth(nextDueDate.Year, nextDueDate.Month))
            {
                nextDueDate = new DateTime(nextDueDate.Year, nextDueDate.Month, DateTime.DaysInMonth(nextDueDate.Year, nextDueDate.Month));
            }
            else
            {
                nextDueDate = new DateTime(nextDueDate.Year, nextDueDate.Month, schedule.DayOfMonth);
            }
            if (schedule.EndDate != null)
            {
                return nextDueDate <= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }
            else
            {
                return nextDueDate;
            }
        }

        private static DateTime GetNextDueDateForMonthlyWeekday(MonthlyWeekdayScheduleDto schedule, DateTime fromDate)
        {
            var nextDueDate = fromDate.AddMonths(schedule.Interval);
            var firstDayOfMonth = new DateTime(nextDueDate.Year, nextDueDate.Month, 1);
            var daysOffset = ((int)schedule.DayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
            nextDueDate = firstDayOfMonth.AddDays(daysOffset + (schedule.WeekOfMonth - 1) * 7);
            if (nextDueDate.Month != firstDayOfMonth.Month)
            {
                nextDueDate = firstDayOfMonth.AddMonths(1).AddDays(-1);
            }
            if (schedule.EndDate != null)
            {
                return nextDueDate <= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }
            else
            {
                return nextDueDate;
            }
        }

        private static DateTime GetNextDueDateForYearlyNumeric(YearlyNumericScheduleDto schedule, DateTime fromDate)
        {
            var nextDueDate = new DateTime(fromDate.Year, schedule.Month, schedule.DayOfMonth);
            if (nextDueDate <= fromDate)
            {
                nextDueDate = nextDueDate.AddYears(schedule.Interval);
            }
            if (schedule.EndDate != null)
            {
                return nextDueDate <= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }
            else
            {
                return nextDueDate;
            }
        }

        private static DateTime GetNextDueDateForYearlyOrdinal(YearlyOrdinalScheduleDto schedule, DateTime fromDate)
        {
            var firstDayOfMonth = new DateTime(fromDate.Year, schedule.Month, 1);
            var daysOffset = ((int)schedule.DayOfWeek - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
            var nextDueDate = firstDayOfMonth.AddDays(daysOffset + (schedule.WeekOfMonth - 1) * 7);
            if (nextDueDate <= fromDate)
            {
                nextDueDate = nextDueDate.AddYears(schedule.Interval);
            }
            if (schedule.EndDate != null)
            {
                return nextDueDate <= schedule.EndDate ? nextDueDate : DateTime.MinValue;

            }
            else
            {
                return nextDueDate;
            }
        }
    }

}
