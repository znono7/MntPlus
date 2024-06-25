using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class ScheduleStore
    {
        public event Action<object?,string>? ScheduleCreated;

        public void CreateSchedule(object? schedule,string message)
        {
            ScheduleCreated?.Invoke(schedule,message);
        }

       
    }
}
