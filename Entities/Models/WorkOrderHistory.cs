using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class WorkOrderHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public DateTime? DateChanged { get; set; }

        [ForeignKey(nameof(ChangedBy))]
        public Guid? ChangedById { get; set; }
        public User? ChangedBy { get; set; }

        [ForeignKey(nameof(WorkOrder))]
        public Guid? WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }
    }
}
