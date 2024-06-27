using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MeterScheduleType
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }
    //generate a list of meter schedule types
    public class MeterScheduleTypes
    {
        public List<MeterScheduleType> MeterScheduleTypeList { get; set; }
        public MeterScheduleTypes()
        {
            MeterScheduleTypeList = new List<MeterScheduleType>
            {
                new MeterScheduleType { Id = 1, Name = "Atteint Chaque" },
                new MeterScheduleType { Id = 2, Name = "est Exactement" },
                new MeterScheduleType { Id = 3, Name = "est Inférieur à" },
                new MeterScheduleType { Id = 4, Name = "est Supérieur à" },
            };
        }
    }
}
