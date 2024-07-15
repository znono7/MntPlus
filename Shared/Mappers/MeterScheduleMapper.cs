using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class MeterScheduleMapper
    {
        public static MeterSchedule Map(MeterScheduleDtoForCreation meter)
        {
            return new MeterSchedule
            {
                FrequencyType = meter.FrequencyType,
                Interval = meter.Interval,
                StartDate = meter.StartDate,
                EndDate = meter.EndDate,
                MeterId = meter.MeterId
            };
        }
         
        public static MeterScheduleDto? Map(MeterSchedule? meter)
        {
            if (meter == null)
            {
                return null;
            }
           
            return new MeterScheduleDto
            (
                Id : meter.Id,
                FrequencyType : meter.FrequencyType,
                Interval : meter.Interval,
                StartDate : meter.StartDate,
                EndDate : meter.EndDate,
                Meter : MeterMapper.Map(meter.Meter)
            );
        }
    }
}
