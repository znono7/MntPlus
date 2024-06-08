using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MeterReadingStore
    {
        public event Action<MeterReadingDto>? MeterReadingUpdated;
        public void UpdateMeterReading(MeterReadingDto meterReading)
        {
            MeterReadingUpdated?.Invoke(meterReading);
        }
    }

    public class MeterStore
    {
        public event Action<MeterDto?>? MeterCreated;
        public event Action<MeterDto?>? MeterUpdated;
        public event Action<MeterDto?>? MeterDeleted;

        public void CreateMeter(MeterDto? meter)
        {
            MeterCreated?.Invoke(meter);
        }

        public void UpdateMeter(MeterDto? meter)
        {
            MeterUpdated?.Invoke(meter);
        }

        public void DeleteMeter(MeterDto? meter)
        {
            MeterDeleted?.Invoke(meter);
        }

       
    }
}
