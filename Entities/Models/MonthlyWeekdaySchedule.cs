using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class MonthlyWeekdaySchedule : Schedule
    {
        [Required]
        public int WeekOfMonth { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }
    }
}
