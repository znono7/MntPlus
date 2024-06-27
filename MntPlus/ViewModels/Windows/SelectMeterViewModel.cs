using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SelectMeterViewModel : MeterPageViewModel
    {
        public MeterScheduleTypes MeterScheduleTypes { get; set; }
        public MeterViewModel SelectedMeter { get; set; }
        public MeterScheduleType SelectedMeterScheduleType { get; set; }

        public ICommand GetSelectedCommand { get; set; }
        public MeterScheduleStore MeterScheduleStore { get; set; }
        public int Interval { get; set; }
        public SelectMeterViewModel(MeterScheduleStore meterScheduleStore)
        {
            MeterScheduleTypes = new MeterScheduleTypes();
            GetSelectedCommand = new RelayParameterizedCommand(GetSelected);
            MeterScheduleStore = meterScheduleStore;

        }

        private void GetSelected(object? obj)
        {
            if(SelectedMeterScheduleType is null || SelectedMeter is null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un type de planification et un compteur"));
                return;
            }
            if(Interval <=  0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ""));
                return;
            }
            if (obj is SelectMeterWindow meter)
            {
                MeterScheduleStore.SelectMeterSchedule(new MeterScheduleDtoForCreation(SelectedMeterScheduleType.Name!, Interval, SelectedMeter.MeterDto.Id),
                    $"Utilisation : Créé quand {SelectedMeter.MeterDto.Name} {Interval} {SelectedMeter.MeterDto.Unit}");
            }
            
        }
    }
}
