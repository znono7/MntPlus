

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
        public EquipmentType? EquipmentType { get; set; }
        public string? EquipmentDescription { get; set; }
        public Organization? EquipmentOrganization { get; set; }
        public EquipmentDepartment? EquipmentDepartment { get; set; }
        public EquipmentClass? EquipmentClass { get; set; }
        public string? EquipmentSite { get; set; }

        public EquipmentStatus? EquipmentStatus { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentSerialNumber { get; set; }

        public string? EquipmentModel { get; set; }
        public double? EquipmentCost { get; set; }
        public DateTime? EquipmentCommissionDate { get; set; }

        public Assignee? EquipmentAssignedTo { get; set; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }


    }
}
