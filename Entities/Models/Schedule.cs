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
        public string? ScheduleType { get; set; }
        public int? Interval { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? LastScheduledDate { get; set; }
        public DateTime? NextScheduledDate { get; set; }

    }
}
