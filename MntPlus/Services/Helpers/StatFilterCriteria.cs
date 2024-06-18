using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class StatFilterCriteria
    {
        public StatFilterType StatFilterType { get; set; }
        public string? FilterValue { get; set; }

        public StatFilterCriteria(StatFilterType filterType, string? filterValue)
        {
            StatFilterType = filterType;
            FilterValue = filterValue;
        }
    }
}
