using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EquipmentStatus
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public string EquipmentStatusName { get; set; }
    }
}
