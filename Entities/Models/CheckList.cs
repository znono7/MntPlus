using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CheckList
    {
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }

       
        public ICollection<CheckListItem>? CheckListItems { get; set; }
    }
}
