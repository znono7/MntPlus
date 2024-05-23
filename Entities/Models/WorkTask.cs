using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class WorkTask
    {
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(WorkOrder))]
        public Guid? PreventiveID { get; set; } 

        public PreventiveMaintenance? PreventiveMaintenance { get; set; }
    }
}
