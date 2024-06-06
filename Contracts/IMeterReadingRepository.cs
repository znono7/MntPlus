using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMeterReadingRepository
    {
        Task<IEnumerable<MeterReading>?> GetAllMeterReadingsAsync(Guid meterId,bool trackChanges);
        Task<MeterReading?> GetMeterReadingAsync(Guid meterReadingId, bool trackChanges);
        void CreateMeterReading(MeterReading meterReading);
        void DeleteMeterReading(MeterReading meterReading);
    }
}
