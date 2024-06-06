using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Meter
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double CurrentReading { get; set; }
        public DateTime LastUpdated { get; set; } 
        public string? Unit { get; set; }
        public int Frequency { get; set; } // e.g., 1, 2, 3
        public string? FrequencyUnit { get; set; } // e.g., "Hours", "Days", "Weeks", "Months"
        public ICollection<MeterReading>? MeterReadings { get; set; }

        [ForeignKey(nameof(Asset))]
        public Guid? AssetId { get; set; }
        public Asset? Asset { get; set; }
    }

}
