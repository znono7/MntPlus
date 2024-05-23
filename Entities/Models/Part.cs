using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Part
    {
        [Key]
        public Guid Id { get; set; }
        public string? PartNumber { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public byte[]? Image { get; set; }
    }
}
