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
        public int? Number { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FrequencyType { get; set; }
        public string? Priority { get; set; }
        public string? Type { get; set; }

        public DateTime? LastPerformed { get; set; }
        public DateTime? NextDue { get; set; }
        public DateTime? DateCreation { get; set; }

        [ForeignKey(nameof(UserCreatedBy))]
        public Guid? UserCreatedId { get; set; }
        public User? UserCreatedBy { get; set; }

        [ForeignKey(nameof(UserAssignedTo))]
        public Guid? UserAssignedToId { get; set; }
        public User? UserAssignedTo { get; set; }

        [ForeignKey(nameof(TeamAssignedTo))]
        public Guid? TeamAssignedToId { get; set; }
        public Team? TeamAssignedTo { get; set; }

        [ForeignKey(nameof(CheckList))]
        public Guid? CheckListId { get; set; }
        public CheckList? CheckList { get; set; }

        [ForeignKey(nameof(Asset))]
        public Guid? AssetId { get; set; }
        public Asset? Asset { get; set; }

        [ForeignKey(nameof(Schedule))]
        public Guid? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

        [ForeignKey(nameof(MeterSchedule))]
        public Guid? MeterScheduleId { get; set; }
        public MeterSchedule? MeterSchedule  { get; set; }

        public ICollection<PreventiveMaintenanceHistory>? PreventiveMaintenanceHistories { get; set; }

    }
}
