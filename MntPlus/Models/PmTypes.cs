using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class PmTypes
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
    }

    public class PmTypesColllection
    {
        public List<PmTypes> PmTypesList { get; set; }

        public PmTypesColllection()
        {
            PmTypesList = new List<PmTypes>
            {
                new PmTypes { Id = 1, TypeName = "Préventif" },
                new PmTypes { Id = 2, TypeName = "lubrification" },
                new PmTypes { Id = 3, TypeName = "Calibration" },
                new PmTypes { Id = 4, TypeName = "Nettoyage" },
                new PmTypes { Id = 5, TypeName = "Réglage" },
                new PmTypes { Id = 6, TypeName = "Remplacement" },
                new PmTypes { Id = 7, TypeName = "Test" },
                new PmTypes { Id = 8, TypeName = "Inspection" },
                new PmTypes { Id = 9, TypeName = "Mise à Niveau" },
                new PmTypes { Id = 10, TypeName = "Dommage" },
                new PmTypes { Id = 11, TypeName = "Sécurité" },
                new PmTypes { Id = 11, TypeName = "Aucun" }
            };
        }
    }
}
