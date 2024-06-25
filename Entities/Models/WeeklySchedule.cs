using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class WeeklySchedule : Schedule
    {
        [Required]
        public List<DayOfWeek>? DaysOfWeek { get; set; }
    }
}
