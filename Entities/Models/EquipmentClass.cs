using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EquipmentClass
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Equipment Class name is a required field.")]
        public string EquipmentClassName { get; set; }
    }
}
