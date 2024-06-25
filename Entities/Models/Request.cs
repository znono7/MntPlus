
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Requester { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdated { get; set; }


        [ForeignKey(nameof(UserCreatedBy))]
        public Guid? UserCreatedId { get; set; }
        public User? UserCreatedBy { get; set; }

        [ForeignKey(nameof(UserAssignedTo))]
        public Guid? UserAssignedToId { get; set; }
        public User? UserAssignedTo { get; set; }

        [ForeignKey(nameof(TeamAssignedTo))]
        public Guid? TeamAssignedToId { get; set; }
        public Team? TeamAssignedTo { get; set; }

        [ForeignKey(nameof(Asset))]
        public Guid? AssetId { get; set; }
        public Asset? Asset { get; set; }


    }
}
