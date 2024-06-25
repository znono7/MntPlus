using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class YearlyNumericSchedule : Schedule
    {
        [Required]
        public int DayOfMonth { get; set; }

        [Required]
        public int Month { get; set; }
    }
}
