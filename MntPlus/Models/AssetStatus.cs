using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class AssetStatus
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
    //generate a list of asset status
    public class AssetStatuses
    {
        public List<AssetStatus> AssetStatusList { get; set; }
        public AssetStatuses()
        {
            AssetStatusList = new List<AssetStatus>
            {
                new AssetStatus { Id = 1, Name = "En Service" },
                new AssetStatus { Id = 2, Name = "Hors Service" },
                new AssetStatus { Id = 3, Name = "En Maintenance" },
                new AssetStatus { Id = 4, Name = "En Stock" },
                new AssetStatus { Id = 5, Name = "En Transit" },
                new AssetStatus { Id = 6, Name = "En Réparation" },
                new AssetStatus { Id = 7, Name = "En Commande" },
                new AssetStatus { Id = 8, Name = "En Attente" },
                new AssetStatus { Id = 9, Name = "En Panne" },
                new AssetStatus { Id = 10, Name = "En Test" },
                new AssetStatus { Id = 11, Name = "En Déplacement" },
                new AssetStatus { Id = 12, Name = "En Stockage" },
            };
        }
    }
}
