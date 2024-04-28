

namespace Entities
{
    public class Team
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<User>? TeamMembers { get; set; }
    }
}
