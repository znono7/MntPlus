using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Inventory
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Part")]
        public Guid PartID { get; set; } 
        public int Quantity { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public double? Cost { get; set; }
        public int? AvailableQuantity { get; set; }
        public int? MinimumQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public DateTime? CreatedOn { get; set; }


        public  Part? Part { get; set; } 
    }
}
