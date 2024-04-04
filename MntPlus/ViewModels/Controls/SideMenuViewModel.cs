using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus
{
    public class SideMenuViewModel : BaseViewModel
    {
        #region Public Properties
        
        public bool TravailMenuEnabled { get; set; }

        public double SideMenuWidth { get; set; } = 59;

        private bool _isMenuOpen;
        public bool IsMenuOpen
        {
            get { return _isMenuOpen; }
            set
            {
                if (_isMenuOpen != value)
                {
                    _isMenuOpen = value;
                    OnPropertyChanged(nameof(IsMenuOpen));

                    // Update SideMenuWidth based on the value of IsMenuOpen
                    if (IsMenuOpen)
                    {
                        SideMenuWidth = 215;
                    }
                    else
                    {
                        SideMenuWidth = 59;
                    }
                }
            }
        }


        #endregion

        #region Commands

        public ICommand TravailCommand { get; set; }
        public ICommand EquipementCommand { get; set; }
        public ICommand DashboardCommand { get; set; }
        public ICommand OpenTaskCommand { get; set; }
        public ICommand CompletedTasksCommand { get; set; }
        public ICommand WorkReqeustCommand { get; set; }

        public ICommand ToggleCommand { get; set; }
        #endregion

        #region Constructor
        public SideMenuViewModel()
        {
            TravailCommand = new RelayCommand(async () => await ToTravailPage() /*TravailMenuEnabled = true*/);//^
            ToggleCommand = new RelayCommand(async () => await ToggleAction());
            DashboardCommand = new RelayCommand(async () => await ToDashPage());
            EquipementCommand = new RelayCommand(async () => await ToEquipementPage());

        }

        private async Task ToTravailPage()
        {
            if(SideMenuWidth == 59)
            {
                return;
            }
           
            TravailMenuEnabled = true;

            await Task.Delay(1);

        }

        private async Task ToggleAction()
        {

            TravailMenuEnabled = false;
            await Task.Delay(1);

        }

        private async Task ToEquipementPage()
        {
            TravailMenuEnabled = false;
            IoContainer.Application.GoToPage(ApplicationPage.Equipement);
            await Task.Delay(1);
        }

        private async Task ToDashPage()
        {
            TravailMenuEnabled = false;
            await Task.Delay(1);
        }
        #endregion
    }
}
