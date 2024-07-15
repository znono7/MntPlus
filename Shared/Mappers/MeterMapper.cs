using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class MeterMapper
    {
        public static Meter Map(MeterDtoForCreation meter)
        {
            return new Meter
            {
                Name = meter.Name,
                CurrentReading = meter.CurrentReading,
                LastUpdated = meter.LastUpdated,
                Unit = meter.Unit,
                Frequency = meter.Frequency,
                FrequencyUnit = meter.FrequencyUnit,
                AssetId = meter.AssetId
            };
        }

        public static MeterDto? Map(Meter? meter)
        {
            if (meter == null)
            {
                return null;
            }
            return new MeterDto
            (Id: meter.Id,
             Name: meter.Name,
             CurrentReading: meter.CurrentReading,
             LastUpdated: meter.LastUpdated,
             Unit: meter.Unit,
             Frequency: meter.Frequency,
             FrequencyUnit: meter.FrequencyUnit,
             AssetId: meter.Asset != null ? meter.AssetId : null,
             AssetName: meter.Asset != null ? meter.Asset!.Name : null,
             null); 
        }
    }
}
