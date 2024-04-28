using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ViewTaskViewModel : BaseViewModel
    {
        public bool IsMenuPrioprityOpen { get; set; }
        public ICommand OpenMenuPriorityCommand { get; set; }
        public ICommand HighPriorityCommand { get; set; }
        public ICommand MediumPriorityCommand { get; set; }
        public ICommand LowPriorityCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }
        public ICommand OpenMenuTypeCommand { get; set; }
        public ICommand OpenMenuAssignedCommand { get; set; }
        public ICommand OpenMenuStatCommand { get; set; }
        public ICommand OpenStatCommand { get; set; }
        public ICommand InProgressCommand { get; set; }
        public ICommand CompleteCommand { get; set; }


        public string ForgroundColor { get; set; } = "429b1f";
        private string _orderWorkPriority = "Faible";
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                if (_orderWorkPriority == value)
                {
                    return;
                }
                _orderWorkPriority = value;
                switch (value)
                {
                    case "Haute":
                        ForgroundColor = "c22528";
                        break;
                    case "Moyenne":
                        ForgroundColor = "ef6a00";
                        break;
                    case "Faible":
                        ForgroundColor = "429b1f";
                        break;
                }
                IsMenuPrioprityOpen = false;
            }
        }

        public string TagStat { get; set; } = "statopen";
        private string _WorkStat = "Ouvrir";
        public string WorkStat
        {
            get => _WorkStat;
            set
            {
                if (_WorkStat == value)
                {
                    return;
                }
                _WorkStat = value;
                switch (value)
                {
                    case "Ouvrir":
                        ForgroundColorStat = "429b1f";
                        TagStat = "statopen";
                        break;
                    case "En Cours":
                        ForgroundColorStat = "ef6a00";
                        TagStat = "statinprogress";
                        IsMenuStatOpen = false;

                        break;
                    case "Complété":
                        ForgroundColorStat = "c22528";
                        TagStat = "statcomplete";
                        IsMenuStatOpen = false;

                        break;
                }
                IsMenuStatOpen = false;
            }
        }

        private DateTime _dueDate = DateTime.Today;
        public DateTime DueDate { get => _dueDate; set => _dueDate = value.Date; }
        public string? ShortDueDate => DueDate.ToShortDateString();

        public bool IsMenuDueDateOpen { get; set; }
        public bool IsMenuTypeOpen { get; set; }
        public bool IsMenuAssignedOpen { get; set; }
        public string ForgroundColorStat { get;  set; } = "429b1f";
        public bool IsMenuStatOpen { get; set; }

        public string WorkType { get; set; } = "Prévu";

        public TextEntryViewModel TaskName { get; set; }

        public ViewTaskViewModel()
        {
            TaskName = new TextEntryViewModel { OriginalText = "Task Name" };
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            OpenMenuTypeCommand = new RelayCommand(() => IsMenuTypeOpen = !IsMenuTypeOpen);
            OpenMenuAssignedCommand = new RelayCommand(() => IsMenuAssignedOpen = !IsMenuAssignedOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Haute");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Moyenne");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Faible");
            OpenMenuStatCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);
            OpenStatCommand = new RelayCommand(() => WorkStat = "Ouvrir");
            InProgressCommand = new RelayCommand(() => WorkStat = "En Cours");
            CompleteCommand = new RelayCommand(() => WorkStat = "Complété");
        }
    }
}
