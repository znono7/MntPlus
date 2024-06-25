using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class MonthlyNumericSchedule : Schedule
    {
        [Required]
        public int DayOfMonth { get; set; }
    }
}
