using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class MeterSchedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? FrequencyType { get; set; }

        [Required] 
        public int Interval { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

       

        [Required]
        [ForeignKey("Meter")]
        public Guid MeterId { get; set; }
        public Meter? Meter { get; set; }
    }
}
