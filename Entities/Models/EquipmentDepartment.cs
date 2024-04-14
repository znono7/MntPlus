using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EquipmentDepartment
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Department name is a required field.")]
        public string DepartmentName { get; set; }
       
    }
}
