using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CheckListItem
    {
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey("CheckList")]
        public Guid? CheckListId { get; set; }
        public CheckList? CheckList { get; set; }

        [ForeignKey("WorkTask")]
        public Guid? IndividualTaskId { get; set; }
        public IndividualTask? IndividualTaskTask { get; set; }


    }
}
