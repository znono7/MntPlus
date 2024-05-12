using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class FilterCriteria
    {
        public EquipmentFilterType EquipmentFilterType { get; set; }
        public string? FilterValue { get; set; }

        public FilterCriteria(EquipmentFilterType filterType, string? filterValue)
        {
            EquipmentFilterType = filterType;
            FilterValue = filterValue;
        }
    }
}
