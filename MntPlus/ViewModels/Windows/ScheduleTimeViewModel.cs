using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ScheduleTimeViewModel : BaseViewModel
    {
        #region Checked Days
        private bool _isDimancheChecked;
        private bool _isLundiChecked;
        private bool _isMardiChecked;
        private bool _isMercrediChecked;
        private bool _isJeudiChecked;
        private bool _isVendrediChecked;
        private bool _isSamediChecked;

        public bool IsDimancheChecked
        {
            get => _isDimancheChecked;
            set
            {
                _isDimancheChecked = value;
                OnPropertyChanged(nameof(IsDimancheChecked));
            }
        }
        public bool IsLundiChecked
        {
            get => _isLundiChecked;
            set
            {
                _isLundiChecked = value;
                OnPropertyChanged(nameof(IsLundiChecked));
            }
        }

        public bool IsMardiChecked
        {
            get => _isMardiChecked;
            set
            {
                _isMardiChecked = value;
                OnPropertyChanged(nameof(IsMardiChecked));
            }
        }
        public bool IsMercrediChecked
        {
            get => _isMercrediChecked;
            set
            {
                _isMercrediChecked = value;
                OnPropertyChanged(nameof(IsMercrediChecked));
            }
        }

        public bool IsJeudiChecked
        {
            get => _isJeudiChecked;
            set
            {
                _isJeudiChecked = value;
                OnPropertyChanged(nameof(IsJeudiChecked));
            }
        }
        public bool IsVendrediChecked
        {
            get => _isVendrediChecked;
            set
            {
                _isVendrediChecked = value;
                OnPropertyChanged(nameof(IsVendrediChecked));
            }
        }

        public bool IsSamediChecked
        {
            get => _isSamediChecked;
            set
            {
                _isSamediChecked = value;
                OnPropertyChanged(nameof(IsSamediChecked));
            }
        }

        public List<DayOfWeek>? DaysOfWeek { get; set; }
        #endregion
        private bool _isDaily { get; set;}
        public bool IsDaily { get { return _isDaily; } 
            set { 
                _isDaily = value; 
                if (value)
                {
                    IsWeekly = false;
                    IsMonthly = false;
                    IsYearly = false;
                    FrequencyType = "Daily";
                    Interval = 0;
                   
                }
               
                OnPropertyChanged("IsDaily"); } }

        private bool _isWeekly { get; set; }
        public bool IsWeekly { get { return _isWeekly; } 
            set { _isWeekly = value; 
                if (value)
                {
                    IsDaily = false;
                    IsMonthly = false;
                    IsYearly = false;
                    FrequencyType = "Weekly";
                    Interval = 0;

                }

                OnPropertyChanged("IsWeekly"); } }
        private bool _isMonthly { get; set; }
        public bool IsMonthly { get { return _isMonthly; } 
            set { 
                _isMonthly = value; 
                if (value)
                {
                    IsWeekly = false;
                    IsDaily = false;
                    IsYearly = false;
                    IsMonthlyWeekday = true;

                }
                OnPropertyChanged("IsMonthly"); } }
        private bool _isYearly { get; set; }
        public bool IsYearly { get { return _isYearly; } 
            set { _isYearly = value; 
                if (value)
                {
                    IsWeekly = false;
                    IsMonthly = false;
                    IsDaily = false;
                    IsYearlyOrdinal = true;

                }
                OnPropertyChanged("IsYearly"); } }

        private bool _isMonthlyWeekday { get; set; }
        public bool IsMonthlyWeekday { 
            get { return _isMonthlyWeekday; } 
                   
            set { 
                if (value)
                {
                    IsMonthlyNumeric = false;
                    FrequencyType = "MonthlyWeekday";
                    Interval = 0;

                }
                _isMonthlyWeekday = value; 
                          
               
                OnPropertyChanged("IsMonthlyWeekday"); } }
        private bool _isMonthlyNumeric { get; set; }
        public bool IsMonthlyNumeric { get { return _isMonthlyNumeric; } 
                   set {
                if (value)
                {
                    IsMonthlyWeekday = false;
                    FrequencyType = "MonthlyNumeric";
                    Interval = 0;

                }
                _isMonthlyNumeric = value; 
                         
                OnPropertyChanged("IsMonthlyNumeric"); } }

        public bool _isYearlyOrdinal { get; set; }
        public bool IsYearlyOrdinal { get { return _isYearlyOrdinal; } 
                   set { _isYearlyOrdinal = value; 
                           if (value)
                {
                    IsYearlyNumeric = false;
                    FrequencyType = "YearlyOrdinal";
                    Interval = 0;


                }
                OnPropertyChanged("IsYearlyOrdinal"); } }
        private bool _isYearlyNumeric { get; set; }
        public bool IsYearlyNumeric { get { return _isYearlyNumeric; } 
                          set { _isYearlyNumeric = value; 
                                      if (value)
                {
                    IsYearlyOrdinal = false;
                    FrequencyType = "YearlyNumeric";
                    Interval = 0;

                }
                OnPropertyChanged("IsYearlyNumeric"); } }
        public ICommand IncrementDaysCommand { get; set; }
        public ICommand DecrementDaysCommand { get; set; }
        public ICommand IncrementWeeksCommand { get; set; }
        public ICommand DecrementWeeksCommand { get; set; }
        public ICommand IncrementMonthsCommand { get; set; }
        public ICommand DecrementMonthsCommand { get; set; }
        public ICommand IncrementYearsCommand { get; set; }
        public ICommand DecrementYearsCommand { get; set; }

        private int _days { get; set; }
        public int Days { get { return _days; } set { if (value <= 0) return; _days = value; OnPropertyChanged("Days"); } }

        private int _weeks { get; set; }
        public int Weeks { get { return _weeks; } set { if (value <= 0) return; _weeks = value; OnPropertyChanged("Weeks"); } }
 
        private int _months { get; set; }
        public int Months { get { return _months; } set { if (value <= 0) return; _months = value; OnPropertyChanged("Months"); } }
      
        private int _years { get; set; }
        public int Years { get { return _years; } set { if (value <= 0) return; _years = value; OnPropertyChanged("Years"); } }
  



        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ShortStartDate => StartDate.HasValue ? StartDate.Value.ToString("dd-MM-yyyy") : null;
        public string? ShortEndDate => EndDate.HasValue ? EndDate.Value.ToString("dd-MM-yyyy") : null;

        public DateTime? StartWeeklyDate { get; set; }
        public DateTime? EndWeeklyDate { get; set; }
        public string? ShortStartWeeklyDate => StartWeeklyDate.HasValue ? StartWeeklyDate.Value.ToString("dd-MM-yyyy") : null;
        public string? ShortEndWeeklyDate => EndWeeklyDate.HasValue ? EndWeeklyDate.Value.ToString("dd-MM-yyyy") : null;

        public DateTime? StartMonthlyDate { get; set; }
        public DateTime? EndMonthlyDate { get; set; }
        public string? ShortStartMonthlyDate => StartMonthlyDate.HasValue ? StartMonthlyDate.Value.ToString("dd-MM-yyyy") : null;
        public string? ShortEndMonthlyDate => EndMonthlyDate.HasValue ? EndMonthlyDate.Value.ToString("dd-MM-yyyy") : null;

        public DateTime? StartYearlyDate { get; set; }
        public DateTime? EndYearlyDate { get; set; }
        public string? ShortStartYearlyDate => StartYearlyDate.HasValue ? StartYearlyDate.Value.ToString("dd-MM-yyyy") : null;
        public string? ShortEndYearlyDate => EndYearlyDate.HasValue ? EndYearlyDate.Value.ToString("dd-MM-yyyy") : null;

        public string? SelectedMonthlyWeekDay { get; set; }
        public List<string> MonthlyWeekDay { get; set; }
        public List<string> MonthlyNumericDay { get; set; }
        public string? SelectedMonthlyNumericDay { get; set; }
        public int? DayOfMonth { get; set; }
        public List<string> WeekDay { get; set; }
        public string? SelectedWeekDay { get; set; }
        public List<string> YearMonths { get; set; }
        public string? SelectedYearMonth { get; set; }

        public ICommand SetScheduleCommand { get; set; }

        public string? FrequencyType { get; set; }
        public int Interval { get; set; }
        public ScheduleStore ScheduleStore { get; set; }

        public ICommand OpenMenuStartDateDays { get; set; }
        public bool IsMenuStartDateDaysOpen { get; set; }
        public ICommand OpenMenuEndDateDays { get; set; }
        public bool IsMenuEndDateDaysOpen { get; set; }


        public ICommand OpenMenuStartDateWeekly { get; set; }
        public bool IsMenuStartDateWeeklyOpen { get; set; }
        public ICommand OpenMenuEndDateWeekly { get; set; }
        public bool IsMenuEndDateWeeklyOpen { get; set; }

        public ICommand OpenMenuStartDateMonthly { get; set; }
        public bool IsMenuStartDateMonthlyOpen { get; set; }
        public ICommand OpenMenuEndDateMonthly { get; set; }
        public bool IsMenuEndDateMonthlyOpen { get; set; }

        public ICommand OpenMenuStartDateYearly { get; set; }
        public bool IsMenuStartDateYearlyOpen { get; set; }
        public ICommand OpenMenuEndDateYearly { get; set; }
        public bool IsMenuEndDateYearlyOpen { get; set; }
        public ScheduleTimeViewModel(ScheduleStore scheduleStore)
        {
            ScheduleStore = scheduleStore;
            SetScheduleCommand = new RelayParameterizedCommand(CreateSchedule);
            IncrementDaysCommand = new RelayCommand(() => Days++);
            DecrementDaysCommand = new RelayCommand(() => Days--) ;
            IncrementWeeksCommand = new RelayCommand(() => Weeks++);
            DecrementWeeksCommand = new RelayCommand(() => Weeks--);
            IncrementMonthsCommand = new RelayCommand(() => Months++);
            DecrementMonthsCommand = new RelayCommand(() => Months--);
            IncrementYearsCommand = new RelayCommand(() => Years++);
            DecrementYearsCommand = new RelayCommand(() => Years--);

            OpenMenuStartDateDays = new RelayCommand(() => IsMenuStartDateDaysOpen = !IsMenuStartDateDaysOpen);
            OpenMenuEndDateDays = new RelayCommand(() => IsMenuEndDateDaysOpen = !IsMenuEndDateDaysOpen);

            OpenMenuStartDateWeekly = new RelayCommand(() => IsMenuStartDateWeeklyOpen = !IsMenuStartDateWeeklyOpen);
            OpenMenuEndDateWeekly = new RelayCommand(() => IsMenuEndDateWeeklyOpen = !IsMenuEndDateWeeklyOpen);

            OpenMenuStartDateMonthly = new RelayCommand(() => IsMenuStartDateMonthlyOpen = !IsMenuStartDateMonthlyOpen);
            OpenMenuEndDateMonthly = new RelayCommand(() => IsMenuEndDateMonthlyOpen = !IsMenuEndDateMonthlyOpen);

            OpenMenuStartDateYearly = new RelayCommand(() => IsMenuStartDateYearlyOpen = !IsMenuStartDateYearlyOpen);
            OpenMenuEndDateYearly = new RelayCommand(() => IsMenuEndDateYearlyOpen = !IsMenuEndDateYearlyOpen);
            
            MonthlyWeekDay = new List<string> { "1er", "2ème", "3ème", "4ème" };
            
            WeekDay = new List<string> { "Dimanche" , "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"};
            MonthlyNumericDay = new List<string> { "1er", "2ème", "3ème", "4ème", "5ème" , "6ème"
                , "7ème", "8ème", "9ème", "10ème", "11ème", "12ème", "13ème", "14ème", "15ème", "16ème", "17ème", "18ème", "19ème", "20ème", "21ème", "22ème", "23ème", "24ème", "25ème", "26ème", "27ème", "28ème", "29ème", "30ème", "31ème"
            };
            YearMonths = new List<string> { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" };
        }

        private void CreateSchedule(object? p)
        {
           
            switch (FrequencyType)
            {
                case "Daily":
                    CreateDailySchedule(Days,p);
                    break;
                case "Weekly":
                    CreateWeeklySchedule( Weeks, p);
                    break;
                case "MonthlyWeekday":
                    CreateMonthlyWeekdaySchedule(Months, SelectedMonthlyWeekDay, SelectedWeekDay, p);
                    break;
                case "MonthlyNumeric":
                    CreateMonthlyNumericSchedule(Months, SelectedMonthlyNumericDay, p);
                    break;
                case "YearlyOrdinal":
                    CreateYearlyOrdinalSchedule(Years, SelectedYearMonth, SelectedMonthlyWeekDay, SelectedWeekDay, p);
                    break;
                case "YearlyNumeric":
                    CreateYearlyNumericSchedule(Years, SelectedYearMonth, SelectedMonthlyNumericDay, p);
                    break;
            }
        }

        private void CreateYearlyNumericSchedule(int interval, string? selectedYearMonth, string? dayOfMonth, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }
            if (selectedYearMonth == null || dayOfMonth == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un mois et un jour du mois"));
                return;
            }
            if (StartYearlyDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if (StartYearlyDate != null && EndYearlyDate != null && StartYearlyDate >= EndYearlyDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
               

                var yearlyNumericSchedule = new YearlyNumericScheduleDtoForCreation(FrequencyType ?? "",
                                                                                    interval,
                                                                                    StartYearlyDate.HasValue ? StartYearlyDate.Value : DateTime.Now,
                                                                                    EndYearlyDate,
                                                                                    MonthlyNumericDay.IndexOf(dayOfMonth) + 1,
                                                                                    YearMonths.IndexOf(selectedYearMonth) + 1);
                ScheduleStore.CreateSchedule(yearlyNumericSchedule, "YearlyNumeric");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier annuel numérique"));

            }
        }

        private void CreateYearlyOrdinalSchedule(int interval, string? selectedYearMonth, string? selectedMonthlyWeekDay,string? selectedWeekDay, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }
            if (selectedYearMonth == null || selectedMonthlyWeekDay == null || selectedWeekDay == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un mois et un jour de la semaine"));
                return;
            }
            if(StartYearlyDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if (StartYearlyDate != null && EndYearlyDate != null && StartYearlyDate >= EndYearlyDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                var yearlyOrdinalSchedule = new YearlyOrdinalScheduleDtoForCreation(FrequencyType ?? "",
                                                                                       interval,
                                                                                       StartYearlyDate.HasValue ? StartYearlyDate.Value : DateTime.Now,
                                                                                       EndYearlyDate,
                                                                                       WeekOfMonth: MonthlyWeekDay.IndexOf(selectedMonthlyWeekDay) + 1,
                                                                                       ConvertFrenchDayToDayOfWeek(selectedWeekDay),
                                                                                       YearMonths.IndexOf(selectedYearMonth) + 1);
                ScheduleStore.CreateSchedule(yearlyOrdinalSchedule, "YearlyOrdinal");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier annuel ordinal"));

            }
        }

        private void CreateMonthlyNumericSchedule(int interval, string? dayOfMonth, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }
            if (dayOfMonth == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un jour du mois"));
                return;
            }
            if(StartMonthlyDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if(StartMonthlyDate != null && EndMonthlyDate != null && StartMonthlyDate >= EndMonthlyDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                var monthlyNumericSchedule = new MonthlyNumericScheduleDtoForCreation(FrequencyType ?? "",
                                                                                         interval,
                                                                                         StartMonthlyDate.HasValue ? StartMonthlyDate.Value : DateTime.Now,
                                                                                         EndMonthlyDate,
                                                                                         MonthlyNumericDay.IndexOf(dayOfMonth) + 1);
                ScheduleStore.CreateSchedule(monthlyNumericSchedule, "MonthlyNumeric");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier mensuel numérique"));
            }
        }

        private void CreateMonthlyWeekdaySchedule(int interval, string? selectedMonthlyWeekDay,string? selectedWeekDay, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }
            if (selectedMonthlyWeekDay == null || selectedWeekDay == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un jour de la semaine et un jour du mois"));
                return;
            }
            if(StartMonthlyDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if(StartMonthlyDate != null && EndMonthlyDate != null && StartMonthlyDate >= EndMonthlyDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                int _index = MonthlyWeekDay.IndexOf(selectedMonthlyWeekDay) + 1;
                var _weekDay = ConvertFrenchDayToDayOfWeek(selectedWeekDay);
                MonthlyWeekdayScheduleDtoForCreation monthlyWeekdaySchedule = new MonthlyWeekdayScheduleDtoForCreation(FrequencyType ?? "",
                                                                                         interval,
                                                                                         StartMonthlyDate.HasValue ? StartMonthlyDate.Value : DateTime.Now,
                                                                                         EndMonthlyDate,
                                                                                         _index,
                                                                                         _weekDay);
                ScheduleStore.CreateSchedule(monthlyWeekdaySchedule, "MonthlyWeekday");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier mensuel par jour de la semaine"));
            }
        }

        private void CreateWeeklySchedule(int interval, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }

            List<DayOfWeek> daysOfWeek = new();
            if (IsDimancheChecked) daysOfWeek.Add(DayOfWeek.Sunday);
            if (IsLundiChecked) daysOfWeek.Add(DayOfWeek.Monday);
            if (IsMardiChecked) daysOfWeek.Add(DayOfWeek.Tuesday);
            if (IsMercrediChecked) daysOfWeek.Add(DayOfWeek.Wednesday);
            if (IsJeudiChecked) daysOfWeek.Add(DayOfWeek.Thursday);
            if (IsVendrediChecked) daysOfWeek.Add(DayOfWeek.Friday);
            if (IsSamediChecked) daysOfWeek.Add(DayOfWeek.Saturday);


            if (daysOfWeek.Count == 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner au moins un jour de la semaine"));
                return;
            }
            if(StartWeeklyDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if(StartWeeklyDate != null && EndWeeklyDate != null && StartWeeklyDate >= EndWeeklyDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                var weeklySchedule = new WeeklyScheduleDtoForCreation(FrequencyType ?? "",
                                                                        interval,
                                                                        StartWeeklyDate.HasValue ? StartWeeklyDate.Value : DateTime.Now,
                                                                        EndWeeklyDate,
                                                                        daysOfWeek);
                ScheduleStore.CreateSchedule(weeklySchedule, "Weekly");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier hebdomadaire"));
            }
        }

        private void CreateDailySchedule(int interval, object? p)
        {
            if (interval <= 0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'intervalle ne peut pas être égal à 0"));
                return;
            }
            if (StartDate == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une date de début"));
                return;
            }
            if(StartDate != null && EndDate != null && StartDate >= EndDate)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                return;
            }
            try
            {
                var dailySchedule = new DailyScheduleDtoForCreation(FrequencyType ?? "",
                                                                       interval,
                                                                       StartDate.HasValue ? StartDate.Value : DateTime.Now,
                                                                       EndDate);
                ScheduleStore.CreateSchedule(dailySchedule, "Daily");
                if (p == null) return;
                var window = (ScheduleTimeWindow)p;
                window.Close();
            }
            catch (Exception)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création du calendrier quotidien"));

            }
        }

        public static DayOfWeek ConvertFrenchDayToDayOfWeek(string frenchDay)
        {
            var frenchToDayOfWeekMap = new Dictionary<string, DayOfWeek>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Dimanche", DayOfWeek.Sunday },
            { "Lundi", DayOfWeek.Monday },
            { "Mardi", DayOfWeek.Tuesday },
            { "Mercredi", DayOfWeek.Wednesday },
            { "Jeudi", DayOfWeek.Thursday },
            { "Vendredi", DayOfWeek.Friday },
            { "Samedi", DayOfWeek.Saturday }
        };

            if (frenchToDayOfWeekMap.TryGetValue(frenchDay, out DayOfWeek dayOfWeek))
            {
                return dayOfWeek;
            }
            else
            {
                throw new ArgumentException("Invalid French day name", nameof(frenchDay));
            }
        }

    }
}
