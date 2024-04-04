using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasData(
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentName = "Equipment 1",
                EquipmentParent = null,
                EquipmentCategory = "Category A",
                EquipmentModel = "Model X",
                EquipmentMake = "Make Y",
                EquipmentNameImage = "equipment1.jpg",
                EquipmentImage = new byte[] { /* Image data */ }
            },
            new Equipment
            {
                Id = Guid.NewGuid(),
                EquipmentName = "Equipment 2",
                EquipmentParent = null, // Set the parent equipment's ID here
                EquipmentCategory = "Category B",
                EquipmentModel = "Model Z",
                EquipmentMake = "Make X",
                EquipmentNameImage = "equipment2.jpg",
                EquipmentImage = new byte[] { /* Image data */ }
            }
        );

        }
    }
}
