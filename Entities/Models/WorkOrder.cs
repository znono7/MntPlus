
namespace Entities
{
    public class WorkOrder
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Instructions { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public User? UserAssignedTo { get; set; }
        public Team? TeamAssignedTo { get; set; }
    }
}
