using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MeterStore
    {
        public event Action<MeterDto?>? MeterCreated;
        public event Action<MeterDto?>? MeterUpdated;
        public void CreateMeter(MeterDto? meter)
        {
            MeterCreated?.Invoke(meter);
        }

        public void UpdateMeter(MeterDto? meter)
        {
            MeterUpdated?.Invoke(meter);
        }
    }
}
