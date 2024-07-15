using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Schedule
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

       
    }
}
