using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record MeterDto
        (
        Guid Id,
        string? Name,
        double CurrentReading, 
        DateTime LastUpdated,
        string? Unit, 
        int Frequency,
        string? FrequencyUnit, 
        Guid? AssetId,
        string? AssetName,
        List<MeterReadingDto>? MeterReadings
        );

    public record MeterDtoForCreation
       (
       string? Name, 
       double CurrentReading,
       DateTime LastUpdated,
       string? Unit,
       int Frequency,
       Guid? AssetId,
       string? FrequencyUnit
       );

}
