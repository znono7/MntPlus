using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Schedule
    {
        [Key]
        public Guid Id { get; set; }

        public int? Interval { get; set; }

        // Daily specific
        public bool IsDaily { get; set; }

        // Weekly specific
        public List<string>? DaysOfWeek { get; set; }

        // Monthly specific (Weekday)
        public string? Week { get; set; }
        public string? DayOfWeek { get; set; }

        // Monthly specific (Numeric)
        public int? DayOfMonth { get; set; }

        // Yearly specific (Ordinal)
        public string? Month { get; set; }

        // Yearly specific (Numeric)
        public int? YearDayOfMonth { get; set; }

    }
}
