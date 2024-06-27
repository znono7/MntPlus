using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MeterScheduleStore
    {
        public event Action<MeterScheduleDtoForCreation?,string?>? MeterScheduleSelected;

        public void SelectMeterSchedule(MeterScheduleDtoForCreation? schedule,string? mes)
        {
            MeterScheduleSelected?.Invoke(schedule,mes);
        }
    }
}
