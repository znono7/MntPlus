using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WorkOrderHistory
    {
        [Key]
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public DateTime? DateChanged { get; set; }

        [ForeignKey(nameof(ChangedBy))]
        public Guid? ChangedById { get; set; }
        public User? ChangedBy { get; set; }
        public string? Notes { get; set; }

        [ForeignKey(nameof(WorkOrder))]
        public Guid? WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }
    }
}
