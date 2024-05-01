using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class AssetType
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
         
    }
    //generate a list of asset types
    public class AssetTypes
    {
        public List<AssetType> AssetTypeList { get; set; }
        public AssetTypes()
        {
            AssetTypeList = new List<AssetType>
            {
                new AssetType { Id = 1, Name = "Équipements" },
                new AssetType { Id = 2, Name = "Équipement de Sécurité" },
                new AssetType { Id = 3, Name = "Installations" },
                new AssetType { Id = 4, Name = "Véhicules" },
                new AssetType { Id = 5, Name = "Actifs informatiques" },
                new AssetType { Id = 6, Name = "Infrastructure " },
                new AssetType { Id = 7, Name = "Outils et Instruments" },
                new AssetType { Id = 8, Name = "Mobilier et Agencements" },
                new AssetType { Id = 9, Name = "Consommables et Pièces de Rechange" },
                new AssetType { Id = 10, Name = "Aménagement Paysager" },
                new AssetType { Id = 11, Name = "Services Publics" },
                new AssetType { Id = 12, Name = "Actifs Personnalisés" },
            };
        }
    }
}
