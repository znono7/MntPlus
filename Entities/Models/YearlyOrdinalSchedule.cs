using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class YearlyOrdinalSchedule : Schedule
    {
        [Required]
        public int WeekOfMonth { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public int Month { get; set; }
    }
}
