using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Request
    {
        [Key]
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public string? Requester { get; set; }
        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(SubmittedBy))]
        public Guid? SubmittedById { get; set; }
        public User? SubmittedBy { get; set; }
    }
}
