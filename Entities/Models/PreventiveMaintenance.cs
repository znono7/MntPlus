using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PreventiveMaintenance
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        [ForeignKey(nameof(Asset))]
        public Guid? AssetId { get; set; }
        public Asset? Asset { get; set; }

        [ForeignKey(nameof(Schedule))]
        public Guid? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

        public ICollection<WorkTask>? Tasks { get; set; }
        public ICollection<PreventiveMaintenanceHistory>? PreventiveMaintenanceHistories { get; set; }

    }
}
