using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ScheduleTimeViewModel : BaseViewModel
    {
        private bool _isDaily { get; set;}
        public bool IsDaily { get { return _isDaily; } 
            set { 
                _isDaily = value; 
                if (value)
                {
                    IsWeekly = false;
                    IsMonthly = false;
                    IsYearly = false;
                   
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
       
                }
                OnPropertyChanged("IsYearly"); } }

        private bool _isMonthlyWeekday { get; set; }
        public bool IsMonthlyWeekday { 
            get { return _isMonthlyWeekday; } 
                   
            set { 
                if (value)
                {
                    IsMonthlyNumeric = false;
                }
                _isMonthlyWeekday = value; 
                          
               
                OnPropertyChanged("IsMonthlyWeekday"); } }
        private bool _isMonthlyNumeric { get; set; }
        public bool IsMonthlyNumeric { get { return _isMonthlyNumeric; } 
                   set {
                if (value)
                {
                    IsMonthlyWeekday = false;
                }
                _isMonthlyNumeric = value; 
                         
                OnPropertyChanged("IsMonthlyNumeric"); } }

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

        public string SelectedMonthlyWeekDay { get; set; }
        public List<string> MonthlyWeekDay { get; set; }
        public List<string> MonthlyNumericDay { get; set; }
        public string SelectedMonthlyNumericDay { get; set; }
        public List<string> WeekDay { get; set; }
        public string SelectedWeekDay { get; set; }
        public ScheduleTimeViewModel()
        {
            IncrementDaysCommand = new RelayCommand(() => Days++);
            DecrementDaysCommand = new RelayCommand(() => Days--) ;
            IncrementWeeksCommand = new RelayCommand(() => Weeks++);
            DecrementWeeksCommand = new RelayCommand(() => Weeks--);
            IncrementMonthsCommand = new RelayCommand(() => Months++);
            DecrementMonthsCommand = new RelayCommand(() => Months--);
            IncrementYearsCommand = new RelayCommand(() => Years++);
            DecrementYearsCommand = new RelayCommand(() => Years--);
            
            MonthlyWeekDay = new List<string> { "1er", "2ème", "3ème", "4ème" };
            WeekDay = new List<string> { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
            MonthlyNumericDay = new List<string> { "1er", "2ème", "3ème", "4ème", "5ème" , "6ème"
                , "7ème", "8ème", "9ème", "10ème", "11ème", "12ème", "13ème", "14ème", "15ème", "16ème", "17ème", "18ème", "19ème", "20ème", "21ème", "22ème", "23ème", "24ème", "25ème", "26ème", "27ème", "28ème", "29ème", "30ème", "31ème"
            };

        }

    }
}
