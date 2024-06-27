using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class NewPreventiveMaintenanceViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Number { get; set; }
        public int num { get; set; } 
        public string ForgroundColor { get; set; } = "53667B"; 
        public bool IsMenuPrioprityOpen { get; set; }
        private string _orderWorkPriority = "Aucune";
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                _orderWorkPriority = value;
                switch (value)
                {
                    case "1":
                        ForgroundColor = "c22528";
                        IsMenuPrioprityOpen = false;
                        break;
                    case "2":
                        ForgroundColor = "ef6a00";
                        IsMenuPrioprityOpen = false;

                        break;
                    case "3":
                        ForgroundColor = "429b1f";
                        IsMenuPrioprityOpen = false;

                        break;
                    case "Aucune":
                        ForgroundColor = "53667B";
                        IsMenuPrioprityOpen = false;
                        break;
                }
            }
        }
        public ICommand BackPageCommand { get; set; }
        public ICommand ScheduleWindCommand { get; set; }
        public ICommand OpenMenuPriorityCommand { get; set; }

        public PmTypesColllection? TypesColllection { get; set; }
        public PmTypes? SelectedType { get; set; }

        public ScheduleStore ScheduleStore { get; set; }
        public string? ScheduleText { get; set; }

        public object? ScheduleModel { get; set; }
        public AssetStore SelectedAssetStore { get; private set; }
        public ICommand BrowseToEquipmentCommand { get; set; }
        public AssetDto? SelectedAsset { get; set; }

        public bool browseToEquipmentVisible { get; set; } = true;
        public string? SelectAssetContent { get; set; } = "Attribuer l'équipement";
        public ICommand ClearEquipmentCommand { get; set; }
        public string? SelectedAssignedTo { get; set; }
        public bool browseToAssignedVisible { get; set; } = true;
        public ICommand BrowseToAssignedCommand { get; set; }
        public ICommand ClearAssignedCommand { get; set; }
        public UserTeamStore? UserTeamStore { get; set; }
        public Guid? UserGuid { get; set; }
        public Guid? TeamGuid { get; set; }

        public ICommand AddTaskCommand { get; set; }
        public CheckListDto? CheckListDto { get; set; }
        public bool IsTaskSelected { get; set; } = false;
        public ICommand ClearTaskCommand { get; set; }

        public ICommand BrowseToMeterCommand { get; set; }
        public ICommand ClearMeterCommand { get; set; }
        public bool browseToMeterVisible { get; set; } = false;
        public bool browseToMeterScheduleVisible { get; set; } = false;
        public MeterScheduleDtoForCreation MeterSchedule { get; set; }
        public MeterScheduleStore MeterScheduleStore { get; set; }
        public string? MeterScheduleText { get; set; }
        public bool IsOrVisible => browseToMeterScheduleVisible && browseToMeterVisible;
        public NewPreventiveMaintenanceViewModel() 
        {
            TypesColllection = new PmTypesColllection();
            BackPageCommand = new RelayCommand(() => BackPage());
            ScheduleWindCommand = new RelayCommand(() => ScheduleWind());
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            BrowseToEquipmentCommand = new RelayCommand(BrowseToEquipment);
            ClearEquipmentCommand = new RelayCommand(() => { SelectedAsset = null; browseToEquipmentVisible = true; SelectAssetContent = "Attribuer l'équipement"; });
            BrowseToAssignedCommand = new RelayCommand(BrowseToAssigned);
            ClearAssignedCommand = new RelayCommand(() => { SelectedAssignedTo = null; browseToAssignedVisible = true; });
            AddTaskCommand = new RelayCommand(AddTask);
            ClearTaskCommand = new RelayCommand(() => { CheckListDto = null; IsTaskSelected = false; });
            ScheduleStore = new ScheduleStore();
            ScheduleStore.ScheduleCreated += ScheduleStore_ScheduleCreated;
            BrowseToMeterCommand = new RelayCommand(BrowseToMeter);
        }

        private void BrowseToMeter()
        {
            MeterScheduleStore = new MeterScheduleStore();
            
            SelectMeterWindow selectMeterWindow = new() { DataContext = new SelectMeterViewModel(MeterScheduleStore) };
        }

        private void AddTask()
        {
            CheckListStore CheckListSelectStore = new CheckListStore();
            CheckListSelectStore.CheckListSelected += CheckListSelectStore_CheckListSelected;
            SelectTaskWindow selectTaskWindow = new SelectTaskWindow() { DataContext = new SelectTaskViewModel(CheckListSelectStore) };
            selectTaskWindow.ShowDialog();
        }

        private void CheckListSelectStore_CheckListSelected(CheckListDto? dto)
        {
            CheckListDto = dto;
            IsTaskSelected = true;


        }

        private void BrowseToAssigned()
        {
            UserTeamStore = new UserTeamStore();
            UserTeamStore.UserTeamSelected += OnSelectedAssigned;
            SelectUserTeamWindow selectUserTeamWindow = new() { DataContext = new SelectUserTeamWindowViewModel(UserTeamStore) };
            selectUserTeamWindow.ShowDialog();
        }

        private void OnSelectedAssigned(UserTeamDto? dto)
        {
            if (dto?.User is null)
            {
                SelectedAssignedTo = $"Équipe : {dto?.Team?.Name}";
                TeamGuid = dto?.Team?.Id ?? Guid.Empty;
                browseToAssignedVisible = false;
            }
            else if (dto?.Team is null)
            {
                SelectedAssignedTo = $"Utilisateur : {dto?.User?.UserDto?.FullName} ";
                UserGuid = dto?.User?.UserDto?.Id ?? Guid.Empty;
                browseToAssignedVisible = false;

            }
        }
        private void BrowseToEquipment()
        {
            SelectedAssetStore = new AssetStore();
            SelectedAssetStore.AssetCreated += OnSelectedEquipment;
            SelectEquipmentWindow selectEquipmentWindow = new SelectEquipmentWindow() { DataContext = new SelectEquipmentViewModel(SelectedAssetStore) };
            selectEquipmentWindow.ShowDialog();
        }
        private void OnSelectedEquipment(AssetDto? dto)
        {
            SelectedAsset = dto;
            browseToEquipmentVisible = false;
            SelectAssetContent = dto?.Name;
        }
        private void ScheduleStore_ScheduleCreated(object? arg1, string arg2)
        {
            if(arg1 == null || arg2 == null) return;
            switch (arg2)
            {
                case "Daily":
                    ScheduleModel = (DailyScheduleDtoForCreation)arg1;
                   
                    break;
                case "Weekly":
                    ScheduleModel = (WeeklyScheduleDtoForCreation)arg1;
                    break;
                case "MonthlyWeekday":
                    ScheduleModel = (MonthlyWeekdayScheduleDtoForCreation)arg1;
                    break;
                case "MonthlyNumeric":
                    ScheduleModel = (MonthlyNumericScheduleDtoForCreation)arg1;
                    break;
                case "YearlyOrdinal":
                    ScheduleModel = (YearlyOrdinalScheduleDtoForCreation)arg1;
                    break;
                case "YearlyNumeric":
                    ScheduleModel = (YearlyNumericScheduleDtoForCreation)arg1;
                    break;
                default:
                    break;
            }
            UseScheduleModel();
            SetScheduleText();
            browseToMeterScheduleVisible = true;
        }
        private void UseScheduleModel()
        {
            switch (ScheduleModel)
            {
                case DailyScheduleDtoForCreation dailySchedule:
                    // Use dailySchedule
                    break;
                case WeeklyScheduleDtoForCreation weeklySchedule:
                    // Use weeklySchedule
                    break;
                case MonthlyWeekdayScheduleDtoForCreation monthlyWeekdaySchedule:
                    // Use monthlyWeekdaySchedule
                    break;
                case MonthlyNumericScheduleDtoForCreation monthlyNumericSchedule:
                    // Use monthlyNumericSchedule
                    break;
                case YearlyOrdinalScheduleDtoForCreation yearlyOrdinalSchedule:
                    // Use yearlyOrdinalSchedule
                    break;
                case YearlyNumericScheduleDtoForCreation yearlyNumericSchedule:
                    // Use yearlyNumericSchedule
                    break;
                default:
                    throw new InvalidOperationException("Unknown schedule type");
            }
        }
        private void SetScheduleText()
        {
               if(ScheduleModel == null) return;
               switch (ScheduleModel)
            {
                case DailyScheduleDtoForCreation dailySchedule:
                    ScheduleText = $"Se répète tous les {dailySchedule.Interval} jour(s)";
                    break;
                case WeeklyScheduleDtoForCreation weeklySchedule:
                    ScheduleText = $"Se répète tous les {weeklySchedule.Interval} semaine(s) le {ConvertDaysOfWeekToFrench(weeklySchedule.DaysOfWeek)}";
                    break;
                case MonthlyWeekdayScheduleDtoForCreation monthlyWeekdaySchedule:
                    ScheduleText = $"Se répète tous les {monthlyWeekdaySchedule.Interval} mois le {ConvertWeekOfMonth(monthlyWeekdaySchedule.WeekOfMonth)} {ConvertDaysOfWeekToFrench(monthlyWeekdaySchedule.DayOfWeek)}";
                    break;
                case MonthlyNumericScheduleDtoForCreation monthlyNumericSchedule:
                    ScheduleText = $"Se répète tous les {monthlyNumericSchedule.Interval} mois le {ConvertDayOfMonth(monthlyNumericSchedule.DayOfMonth)}";
                    break;
                case YearlyOrdinalScheduleDtoForCreation yearlyOrdinalSchedule:
                    ScheduleText = $"Se répète tous les {yearlyOrdinalSchedule.Interval} ans le  {ConvertWeekOfMonth(yearlyOrdinalSchedule.WeekOfMonth)} {ConvertDaysOfWeekToFrench(yearlyOrdinalSchedule.DayOfWeek)} de {ConvertIndexToMonth(yearlyOrdinalSchedule.Month)}";
                    break;
                case YearlyNumericScheduleDtoForCreation yearlyNumericSchedule:
                    ScheduleText = $"Se répète tous les {yearlyNumericSchedule.Interval} ans en {ConvertIndexToMonth(yearlyNumericSchedule.Month)} le {ConvertDayOfMonth(yearlyNumericSchedule.DayOfMonth)}";
                    break;
                default:
                    throw new InvalidOperationException("Unknown schedule type");
            }
        }
        private string ConvertDaysOfWeekToFrench(DayOfWeek daysOfWeek)
        {
            switch (daysOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Lundi";
                case DayOfWeek.Tuesday:
                    return "Mardi";
                case DayOfWeek.Wednesday:
                    return "Mercredi";
                case DayOfWeek.Thursday:
                    return "Jeudi";
                case DayOfWeek.Friday:
                    return "Vendredi";
                case DayOfWeek.Saturday:
                    return "Samedi";
                case DayOfWeek.Sunday:
                    return "Dimanche";
                default:
                    throw new InvalidOperationException("Invalid day of week");
            }

        }

        private string ConvertDaysOfWeekToFrench(List<DayOfWeek> daysOfWeek)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dayOfWeek in daysOfWeek)
            {
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        sb.Append("Lundi, ");
                        break;
                    case DayOfWeek.Tuesday:
                        sb.Append("Mardi, ");
                        break;
                    case DayOfWeek.Wednesday:
                        sb.Append("Mercredi, ");
                        break;
                    case DayOfWeek.Thursday:
                        sb.Append("Jeudi, ");
                        break;
                    case DayOfWeek.Friday:
                        sb.Append("Vendredi, ");
                        break;
                    case DayOfWeek.Saturday:
                        sb.Append("Samedi, ");
                        break;
                    case DayOfWeek.Sunday:
                        sb.Append("Dimanche, ");
                        break;
                    default:
                        throw new InvalidOperationException("Invalid day of week");
                }
            }
            return sb.ToString().TrimEnd(',', ' ');
          
        
        }
        private string ConvertIndexToMonth(int monthIndex)
        {
            switch (monthIndex)
            {
                case 1:
                    return "Janvier";
                case 2:
                    return "Février";
                case 3:
                    return "Mars";
                case 4:
                    return "Avril";
                case 5:
                    return "Mai";
                case 6:
                    return "Juin";
                case 7:
                    return "Juillet";
                case 8:
                    return "Août";
                case 9:
                    return "Septembre";
                case 10:
                    return "Octobre";
                case 11:
                    return "Novembre";
                case 12:
                    return "Décembre";
                default:
                    throw new InvalidOperationException("Invalid month index");
            }
        }
        private string ConvertWeekOfMonth(int weekOfMonth)
        {
            switch (weekOfMonth)
            {
                case 1:
                    return "1er";
                case 2:
                    return "2ème";
                case 3:
                    return "3ème";
                case 4:
                    return "4ème";
                case 5:
                    return "5ème";
                default:
                    throw new InvalidOperationException("Invalid week of month");
            }
        }
        private string ConvertDayOfMonth(int dayOfMonth)
        {
             if(dayOfMonth == 1) return "1er";
             return $"{dayOfMonth}ème";
        }
            private void BackPage()
        {
            IoContainer.Application.GoToPage(ApplicationPage.PreventiveMaintenance);
        }

        private void ScheduleWind()
        {
            ScheduleTimeWindow scheduleTimeWindow = new () { DataContext = new ScheduleTimeViewModel(ScheduleStore) };
            scheduleTimeWindow.ShowDialog();
        }
    }
}
