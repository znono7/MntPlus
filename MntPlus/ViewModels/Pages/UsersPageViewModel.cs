using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class UsersPageViewModel : BaseViewModel
    {
        public ObservableCollection<UserWithRolesDto>? FilterUsers { get; set; }
        private ObservableCollection<UserWithRolesDto>? users { get; set; }
        public ObservableCollection<UserWithRolesDto>? Users
        { 
            get => users; 
            set {
                users = value; 
                if (users is not null)
                    FilterUsers = new ObservableCollection<UserWithRolesDto>(users);
            }  
        } 
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

        public UsersPageViewModel()
        {
            MenuActionCommand = new RelayCommand(() => IsMenuActionOpen = !IsMenuActionOpen);
            ToUsersPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Users));
            ToTeamsPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Teams));
            OpenWindowCommand = new RelayCommand(OpenUserWindow);
            DeleteUsersCommand = new RelayCommand(RemoveSelectedItems);
            SearchCommand = new RelayCommand(Search);
            AssigneRoleCommand = new RelayParameterizedCommand((p) => AssigneRole(p));
            //_ = GetUsers();
            GetUsers().GetAwaiter().GetResult();
            UserStore = new UserStore();
            UserStore.UserCreated += UserStore_UserCreated;
            RoleStore = new RoleStore();
            RoleStore.UserRoleAssigned += RoleStore_UserRoleAssigned;
        }

        private void AssigneRole(object? p)
        {
            var user = p as UserWithRolesDto;
            if (user is null) { return; }
            AssignNewRole assignNewRole = new AssignNewRole() { DataContext = new AssignNewRoleViewModel(user.UserDto.Id, RoleStore,user.UserRoles) };
            assignNewRole.ShowDialog();
        }

        private void RoleStore_UserRoleAssigned(UserRoleDto? dto)
        {
            var user = Users?.FirstOrDefault(u => u.UserDto?.Id == dto?.UserId);
            if (user is null) { return; }
            user.UserRoles?.Add(dto?.Role);
        }

        private void UserStore_UserCreated(UserWithRolesDto? dto)
        {
            if (dto == null) { return; }
            Users ??= new ObservableCollection<UserWithRolesDto>();
            FilterUsers ??= new ObservableCollection<UserWithRolesDto>();
            Users.Add(dto);
            FilterUsers.Add(dto);
            
        }

        private async Task GetUsers()
        {
            var result = await AppServices.ServiceManager.UserService.GetAllUsersAsync(false);
            if (result.Success && result is ApiOkResponse<IEnumerable<UserWithRolesDto>> response )
            {
                Users = new ObservableCollection<UserWithRolesDto>(response.Result!);
            }
            else if ( result is ApiBadRequestResponse r)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la récupération des utilisateurs"));
                Users = new ObservableCollection<UserWithRolesDto>();

            }else if ( result is ApiNotFoundResponse n)
            {
                MessageBox.Show("Aucun utilisateur trouvé");
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info,"Aucun utilisateur trouvé"));
                Users = new ObservableCollection<UserWithRolesDto>();
            }
           
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

        private void RemoveSelectedItems()
        {
            if (FilterUsers is null || FilterUsers.Count == 0)
                return;
            var selectedItems = FilterUsers.Where(item => item.UserDto.IsChecked).ToList();
            foreach (var item in selectedItems)
            {
                FilterUsers.Remove(item);
            }
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
                FilterUsers = new ObservableCollection<UserWithRolesDto>(Users ?? Enumerable.Empty<UserWithRolesDto>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterUsers = new ObservableCollection<UserWithRolesDto>(
                Users.Where(item => item.UserDto.FullName is not null && item.UserDto.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

    }
}
