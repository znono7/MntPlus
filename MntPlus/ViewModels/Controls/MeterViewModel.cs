using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MeterViewModel : BaseViewModel
    {
        public bool IsChecked { get; set; }
        public DateTime? NextReading { get; set; } 
      
        public MeterDto MeterDto { get; }
         
        public string? CurrentReading { get; set; }

        public string? AssetName { get; set;}

        
        public MeterViewModel(MeterDto meterDto)
        {
            MeterDto = meterDto;
            NextReading = CalculateNextReadingTime(meterDto);
            CurrentReading= meterDto.CurrentReading == 0 ? "Pas de lecture encore" : meterDto.CurrentReading.ToString();
            AssetName = meterDto.AssetName is null ? "pas d'équipement" : meterDto.AssetName;
        }

        public DateTime CalculateNextReadingTime(MeterDto meter)
        {
            var nextReadingTime = meter.LastUpdated;

            switch (meter.FrequencyUnit)
            {
                case "Heures":
                    nextReadingTime = nextReadingTime.AddHours(meter.Frequency);
                    break;
                case "Jours":
                    nextReadingTime = nextReadingTime.AddDays(meter.Frequency);
                    break;
                case "Semaines":
                    nextReadingTime = nextReadingTime.AddDays(meter.Frequency * 7);
                    break;
                case "Mois":
                    nextReadingTime = nextReadingTime.AddMonths(meter.Frequency);
                    break;
                default:
                    throw new ArgumentException("Unité de fréquence non valide");
            }

            return nextReadingTime;
        }

    }
}
