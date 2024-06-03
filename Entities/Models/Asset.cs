using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Asset
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? AssetParent { get; set; }
        public Asset? Parent { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }

        [ForeignKey(nameof(Location))]
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public double? PurchaseCost { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? AssetImage { get; set; }
        public DateTime? AssetCommissionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }

        public ICollection<Asset>? Assets { get; set; }
        public ICollection<LinkPart>? LinkParts { get; set; }


    }
}
