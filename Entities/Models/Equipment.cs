

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
   public class Equipment
    {
        [Column("EquipmentId")]
        public Guid Id { get; set; }
        public Guid? EquipmentParent { get; set; }

        [Required(ErrorMessage = "Equipment name is a required field.")]
        public string EquipmentName { get; set; }
        public string? EquipmentCategory { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }


    }
}
