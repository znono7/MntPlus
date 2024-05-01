

using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ICollection<User>? TeamMembers { get; set; }
    }
}
