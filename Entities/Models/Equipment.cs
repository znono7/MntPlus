

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
        public string? EquipmentType { get; set; }
        public string? EquipmentCategory { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentSerialNumber { get; set; }
        public string? EquipmentDescription { get; set; }
        public string? EquipmentLocation { get; set; }
        public EquipmentStatus? EquipmentStatus { get; set; }
        public string? EquipmentCondition { get; set; }
        public string? EquipmentWarranty { get; set; }
        public Organization? EquipmentOrganization { get; set; }
        public Department? EquipmentDepartment { get; set; }
        public EquipmentClass? EquipmentClass { get; set; }
        public string? EquipmentAssignedTo { get; set; }
        public string? EquipmentCommissionDate { get; set; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }


    }
}
