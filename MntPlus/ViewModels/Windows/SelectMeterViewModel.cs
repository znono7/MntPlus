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
        public DateTime? StartDate { get; set; }
        public string? StartDateText => StartDate?.ToString("dd/MM/yyyy");
        public ICommand OpenMenuStartDate { get; set; }
        public bool IsStartDateOpen { get; set; }
        public DateTime? EndDate { get; set; }
        public string? EndDateText => EndDate?.ToString("dd/MM/yyyy");
        public bool IsEndDateOpen { get; set; }
        public ICommand OpenMenuEndDate { get; set; }
        public SelectMeterViewModel(MeterScheduleStore meterScheduleStore)
        {
            MeterScheduleTypes = new MeterScheduleTypes();
            GetSelectedCommand = new RelayParameterizedCommand(GetSelected);
            MeterScheduleStore = meterScheduleStore;
            OpenMenuStartDate = new RelayCommand(() => IsStartDateOpen = !IsStartDateOpen);
            OpenMenuEndDate = new RelayCommand(() => IsEndDateOpen = !IsEndDateOpen);

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
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0\""));
                return;
            }
            if(StartDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if (StartDate != null && EndDate != null && StartDate >= EndDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                if (obj is SelectMeterWindow meter)
                {
                    MeterScheduleStore.SelectMeterSchedule(new MeterScheduleDtoForCreation(SelectedMeterScheduleType.Name!,
                                                                                           Interval,
                                                                                           StartDate.HasValue ? StartDate.Value : DateTime.Now,
                                                                                           EndDate.HasValue ? EndDate.Value : null,
                                                                                           SelectedMeter.MeterDto.Id),
                        $"Utilisation : Créé quand {SelectedMeter.MeterDto.Name} , {Interval} {SelectedMeter.MeterDto.Unit}");
                    meter.Close();
                }
            }
            catch (Exception ex)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ex.Message));
            }
            
        }
    }
}
