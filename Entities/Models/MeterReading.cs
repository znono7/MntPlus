using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class MeterReading
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Meter")]
        public Guid MeterId { get; set; }
        public double Reading { get; set; }
        public DateTime Timestamp { get; set; }


        public Meter? Meter { get; set; }

        [ForeignKey("User")] 
        public Guid? UserId { get; set; }

        public User? User { get; set; }
    }
}
