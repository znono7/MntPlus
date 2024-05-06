using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Instruction
    {
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(WorkOrder))]
        public Guid? WorkOrderID { get; set; } 

        public  WorkOrder? WorkOrder { get; set; }
    }
}
