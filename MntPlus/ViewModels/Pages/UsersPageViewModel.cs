using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class UsersPageViewModel : BaseViewModel
    {
        public ObservableCollection<UserRecordViewModel>? Users
        {
            get => users;
            set
            {
                users = value;
                if (users is not null)
                    FilterUsers = new ObservableCollection<UserRecordViewModel>(users);
            }
        }
        public ObservableCollection<UserRecordViewModel>? FilterUsers { get; set; }
        private ObservableCollection<UserRecordViewModel>? users { get; set; }
      
        private ApplicationPage _currentPage;

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
        public bool IsMenuActionOpen { get; set; }
        protected string? mLastSearchText;
        protected string? mSearchText;
        public string? SearchText
        {
            get => mSearchText;
            set
            {
                // Check value is different
                if (mSearchText == value)
                    return;

                // Update value
                mSearchText = value;

                // If the search text is empty...
                if (string.IsNullOrEmpty(SearchText))
                    // Search to restore messages
                    Search();
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand MenuActionCommand { get; set; }
        public ICommand ToUsersPageCommand { get; set; }
        public ICommand ToTeamsPageCommand { get; set; }
        public ICommand OpenWindowCommand { get; set; }
        public ICommand DeleteUsersCommand { get; set; }
        public ICommand AssigneRoleCommand { get; set; }

        public UserStore? UserStore { get; set; }
        public RoleStore? RoleStore { get; set; }
        public List<RolesDto>? Roles { get; set; }
        public bool IsLoading { get; set; }
        public bool IsEmpty { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public string? UsersCount { get; set;}
        public UsersPageViewModel()
        {
            MenuActionCommand = new RelayCommand(() => IsMenuActionOpen = !IsMenuActionOpen);
            ToUsersPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Users));
            ToTeamsPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Teams));
            OpenWindowCommand = new RelayCommand(OpenUserWindow);
            SearchCommand = new RelayCommand(Search);
            AssigneRoleCommand = new RelayParameterizedCommand((p) => AssigneRole(p));
            LoadDataCommand = new RelayCommand(async () => await GetData());
           
        }

        private async Task GetData()
        {
            await RunCommandAsync(() => IsLoading, async () =>
            {
                var response = await AppServices.ServiceManager.UserService.GetAllUsersWithRolesAsync(false);
                if (response.Success && response is ApiOkResponse<IEnumerable<RolesDto>> result)
                {
                    Roles = new List<RolesDto>(result.Result!);
                    Users = new ObservableCollection<UserRecordViewModel>(Roles.Select(r => new UserRecordViewModel(r)));
                    UsersCount = $"Affichage de {Users.Count} sur {Users.Count} utilisateurs au total" ;
                }else if(response is ApiNotFoundResponse)
                {
                    IsEmpty = true;
                    Users = new ObservableCollection<UserRecordViewModel>();
                    UsersCount = $"Affichage de 0 sur 0 utilisateurs au total";

                }
                else
                {
                    Users = new ObservableCollection<UserRecordViewModel>();
                }
            });
           
        }
        private void AssigneRole(object? p)
        {
            var user = p as UserWithRolesDto;
            if (user is null) { return; }
            AssignNewRole assignNewRole = new AssignNewRole() { DataContext = new AssignNewRoleViewModel(user.UserDto.Id, RoleStore,user.UserRoles) };
            assignNewRole.ShowDialog();
        }

      
       
        private void OpenUserWindow()
        {
            AddUserWindow addUserWindow = new () { DataContext = new AddUserViewModel(UserStore)};
            addUserWindow.ShowDialog();
            
        }

        private async Task NavigateToPageAsync(ApplicationPage page)
        {
            if (CurrentPage == page)
                return;

            CurrentPage = page;
            await Task.Delay(1);
        }

      

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || Users is null || Users.Count <= 0)
            {
                // Make filtered list the same
                FilterUsers = new ObservableCollection<UserRecordViewModel>(Users ?? Enumerable.Empty<UserRecordViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterUsers = new ObservableCollection<UserRecordViewModel>(
                Users.Where(item => item.FullName is not null && item.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

    }
}
