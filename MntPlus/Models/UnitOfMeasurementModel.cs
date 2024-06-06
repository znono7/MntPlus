using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class UnitOfMeasurementModel
    {
        public UnitMeasurement Value { get; set; }
        public string? DisplayName { get; set; }
    }

    public class FrequencyUnitModel
    {
        public FrequencyUnit Value { get; set; }
        public string? DisplayName { get; set; }
    }

}
