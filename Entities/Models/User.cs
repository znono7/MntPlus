

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }

        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; } 
        public DateTime? CreatedAt { get; set; }

        public ICollection<UserTeam>? UserTeams { get; set; } 

        public IEnumerable<UserRole>? UserRoles { get; set; }
       

    }
}
