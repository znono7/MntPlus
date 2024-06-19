using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class PriorityFilterCriteria
    {
        public PriorityFilterType PriorityFilterType { get; set; }
        public string? FilterValue { get; set; }

        public PriorityFilterCriteria(PriorityFilterType filterType, string? filterValue)
        {
            PriorityFilterType = filterType;
            FilterValue = filterValue;
        }
    }
}
