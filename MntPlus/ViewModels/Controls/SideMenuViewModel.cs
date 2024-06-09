using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SideMenuViewModel : BaseViewModel
    {
        #region Public Properties

        private ApplicationPage _currentPage = IoContainer.Application.CurrentPage;

        public ApplicationPage CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    IoContainer.Application.GoToPage(value);
                }
            }
        }

        public bool TravailMenuEnabled { get; set; }

        public double SideMenuWidth { get; set; } = 68;

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
                        SideMenuWidth = 250;
                    }
                    else
                    {
                        SideMenuWidth = 69;
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
        public ICommand TeamsCommand { get; set; }
        public ICommand CompletedTasksCommand { get; set; }
        public ICommand WorkReqeustCommand { get; set; }

        public ICommand ToggleCommand { get; set; }

        public ICommand SettingsCommand { get; set; }
        public ICommand LocationCommand { get; set; }
        public ICommand RequestsCommand { get; set; }
        public ICommand PmCommand { get; set; }
        public ICommand PartsInventoryCommand { get; set; }
        public ICommand CheckListCommand { get; set; }
        public ICommand MetersCommand { get; set; }
        #endregion

        #region Constructor
        public SideMenuViewModel() 
        {  
            TravailCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.ManageWork) /*TravailMenuEnabled = true*/);//^
            ToggleCommand = new RelayCommand(async () => await ToggleAction());
            DashboardCommand = new RelayCommand(async () => await ToDashPage());
            EquipementCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Assets));
            TeamsCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Users));
            SettingsCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Settings));
            LocationCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Locations));
            RequestsCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Requests));
            PmCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.PreventiveMaintenance));
            PartsInventoryCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.PartsInventory));
            CheckListCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Checklists));
            MetersCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Meters));

        }

        private async Task NavigateToPageAsync(ApplicationPage page)
        {
            if (CurrentPage == page)
                return;

            CurrentPage = page;
            await Task.Delay(1);
        }

        private async Task ToTravailPage()
        {
           

            TravailMenuEnabled = true;
            
            IoContainer.Application.GoToPage(ApplicationPage.ManageWork);

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
            IoContainer.Application.GoToPage(ApplicationPage.Assets);
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
