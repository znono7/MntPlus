using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record EquipmentDto
    (
         Guid Id ,
         Guid? EquipmentParent ,
         string EquipmentName ,
         EquipmentTypeDto? EquipmentType ,
         string? EquipmentDescription ,
         EquipmentOrganizationDto? EquipmentOrganization ,
         EquipmentDepartmentDto? EquipmentDepartment , 
         EquipmentClassDto? EquipmentClass ,
         string? EquipmentSite ,
         EquipmentStatusDto? EquipmentStatus ,
         string? EquipmentMake ,
         string? EquipmentSerialNumber ,
         string? EquipmentModel ,
         double? EquipmentCost ,
         DateTime? EquipmentCommissionDate ,
         EquipmentAssignedToDto? EquipmentAssignedTo ,
         string? EquipmentNameImage ,
         byte[]? EquipmentImage 
    );

    public record EquipmentForCreationDto
   ( 

         Guid? EquipmentParent,
         string EquipmentName,
         EquipmentTypeDto? EquipmentType,
         string? EquipmentDescription,
         EquipmentOrganizationDto? EquipmentOrganization, 
         EquipmentDepartmentDto? EquipmentDepartment,
         EquipmentClassDto? EquipmentClass,
         string? EquipmentSite,
         EquipmentStatusDto? EquipmentStatus,
         string? EquipmentMake,
         string? EquipmentSerialNumber,
         string? EquipmentModel,
         double? EquipmentCost,
         DateTime? EquipmentCommissionDate,
         EquipmentAssignedToDto? EquipmentAssignedTo,
         string? EquipmentNameImage,
         byte[]? EquipmentImage
   );





}
