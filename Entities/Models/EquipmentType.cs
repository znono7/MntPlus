using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EquipmentType
    { 
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Type name is a required field.")]
        public string EquipmentTypeName { get; set; }
    }
}
