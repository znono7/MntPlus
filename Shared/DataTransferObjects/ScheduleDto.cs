using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record ScheduleDto (Guid Id, int? Interval, bool IsDaily, List<string>? DaysOfWeek, 
        string? Week, string? DayOfWeek, int? DayOfMonth, string? Month, int? YearDayOfMonth);

    public record CreateScheduleDto(int? Interval, bool IsDaily, List<string>? DaysOfWeek,
       string? Week, string? DayOfWeek, int? DayOfMonth, string? Month, int? YearDayOfMonth);


}
