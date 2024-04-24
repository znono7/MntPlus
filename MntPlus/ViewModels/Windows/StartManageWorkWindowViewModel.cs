using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class StartManageWorkWindowViewModel : BaseViewModel
    {
        public string OrderWorkName { get; set; }
        public string? OrderWorkInstructions { get; set; }

        private string _orderWorkPriority = "3";
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
                }
            }
        }

        private DateTime _dueDate = DateTime.Today;
        public DateTime DueDate { get => _dueDate; set => _dueDate = value.Date; }
        public string? ShortDueDate => DueDate.ToShortDateString();

        public string Type { get; set; } = "Prévu";
        public bool IsMenuPrioprityOpen { get; set; }
        public bool IsMenuDueDateOpen { get; set; }

        public ICommand OpenMenuPriorityCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }

        public ICommand HighPriorityCommand { get; set; }
        public ICommand MediumPriorityCommand { get; set; }
        public ICommand LowPriorityCommand { get; set; }

        public string ForgroundColor { get; set; } = "429b1f";

        public StartManageWorkWindowViewModel()
        {
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "1");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "2");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "3");

        }
    }
}
