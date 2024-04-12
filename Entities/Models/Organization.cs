using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Organization
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Organization name is a required field.")]
        public string OrganizationName { get; set; }
       
    }
}
