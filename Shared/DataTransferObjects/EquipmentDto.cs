using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record EquipmentDto 
        (
        Guid Id, 
        Guid? EquipmentParent, 
        string EquipmentName, 
        string? EquipmentCategory, 
        string? EquipmentModel, 
        string? EquipmentMake, 
        string? EquipmentNameImage, 
        byte[]? EquipmentImage
        );

    public record EquipmentForCreationDto 
        (
        Guid? EquipmentParent,
        string EquipmentName,
        string? EquipmentCategory,
        string? EquipmentModel,
        string? EquipmentMake,
        string? EquipmentNameImage,
        byte[]? EquipmentImage
        );
  
}
