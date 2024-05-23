using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class LinkPart
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Part")]
        public Guid PartId { get; set; }

        [ForeignKey("Asset")]
        public Guid AssetId { get; set; }
        public Part? Part { get; set; }
        public Asset? Asset { get; set; }
    }
}
