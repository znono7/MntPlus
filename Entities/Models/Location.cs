using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public bool IsPrimaryLocation { get; set; }


        [ForeignKey(nameof(Parent))]
        public Guid? IdParent { get; set; }
        public Location? Parent { get; set; }
        public DateTime? CreatedAt { get; set; }

       public ICollection<Location>? Children { get; set; }
    }
}
