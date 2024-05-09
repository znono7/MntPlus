using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class EquipmentCategory
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
    //generate a list of equipment category
    public class EquipmentCategories
    {
        public List<EquipmentCategory> EquipmentCategoryList { get; set; }
        public EquipmentCategories()
        {
            EquipmentCategoryList = new List<EquipmentCategory>
            {
                new EquipmentCategory { Id = 1, Name = "Production/manufacturiers" },
                new EquipmentCategory { Id = 2, Name = "Transport" },
                new EquipmentCategory { Id = 3, Name = "Informatiques et de Bureau" },
                new EquipmentCategory { Id = 4, Name = "Laboratoire" },
                new EquipmentCategory { Id = 5, Name = "HVAC (Chauffage, Ventilation et Climatisation)" },
                new EquipmentCategory { Id = 6, Name = "Sécurité" },
                new EquipmentCategory { Id = 7, Name = "Manutention et de Stockage" },
                new EquipmentCategory { Id = 8, Name = "Médicaux" },
                new EquipmentCategory { Id = 9, Name = "Infrastructure" },
                new EquipmentCategory { Id = 10, Name = "Télécommunication" },
                //new EquipmentCategory { Id = 11, Name = "Tablette" },
                //new EquipmentCategory { Id = 12, Name = "Ecran" },
                new EquipmentCategory { Id = 13, Name = "Autre" },
            };
        }
    }
}
