using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SelectUserTeamWindowViewModel : BaseViewModel
    {
        public ObservableCollection<TeamDto>? teams { get; set; }
        public ObservableCollection<TeamDto>? FilterTeams { get; set; }
        public ObservableCollection<TeamDto>? Teams
        {
            get => teams;
            set
            {
                if (value == teams)
                    return;
                teams = value;
                if(teams != null && teams.Count > 0)
                    FilterTeams = new ObservableCollection<TeamDto>(teams);
                OnPropertyChanged(nameof(Teams));
            }

        }
        public ObservableCollection<UserWithRolesDto>? FilterUsers { get; set; }
        private ObservableCollection<UserWithRolesDto>? users { get; set; }
        public ObservableCollection<UserWithRolesDto>? Users
        {
            get => users;
            set
            {
                if (value == users)
                    return;
                users = value;
                if(users != null && users.Count > 0)
                    FilterUsers = new ObservableCollection<UserWithRolesDto>(users);
                OnPropertyChanged(nameof(Users));
            }

        }
        public UserWithRolesDto? SelectedUser { get; set; }
        public TeamDto? SelectedTeam { get; set; }

        protected string? mLastSearchText;
        protected string? mSearchText;
        protected string? mTeamLastSearchText;
        protected string? mTeamSearchText;
        public ICommand SearchCommand { get; set; }
        public ICommand TeamSearchCommand { get; set; }
        public ICommand SelectUserTeamCommand { get; set; }
        public UserTeamStore? UserTeamStore { get; set; }

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
        public string? TeamSearchText
        {
            get => mTeamLastSearchText;
            set
            {
                // Check value is different
                if (mTeamLastSearchText == value)
                    return;

                // Update value
                mTeamLastSearchText = value;

                // If the search text is empty...
                if (string.IsNullOrEmpty(TeamSearchText))
                    // Search to restore messages
                    TeamSearch();
            }
        }
        public ICommand GetUserTeamCommand { get; set; }
        public SelectUserTeamWindowViewModel(UserTeamStore? userTeamStore)
        {
            _ = GetUsers();
            UserTeamStore = userTeamStore;
            SearchCommand = new RelayCommand(Search);
            TeamSearchCommand = new RelayCommand(TeamSearch);
            SelectUserTeamCommand = new RelayParameterizedCommand((p) => SelectUserTeam(p));
            GetUserTeamCommand = new RelayParameterizedCommand((p) => GetUserTeam(p));
           
        }
        private async Task GetUsers()
        {
            var result = await AppServices.ServiceManager.UserService.GetAllUsersAsync(false);
            if (result.Success && result is ApiOkResponse<IEnumerable<UserWithRolesDto>> response)
            {
                Users = new ObservableCollection<UserWithRolesDto>(response.Result!);
            }
            else if (result is ApiBadRequestResponse r)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la récupération des utilisateurs"));
                Users = new ObservableCollection<UserWithRolesDto>();

            }
            else if (result is ApiNotFoundResponse n)
            {
               
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun utilisateur trouvé"));
                Users = new ObservableCollection<UserWithRolesDto>();
            }

        }

        private void GetUserTeam(object? p)
        {
            if(p is TeamDto team)
            {
                SelectedTeam = team;
                SelectedUser = null;
                UserTeamStore?.SelectUserTeam(new UserTeamDto(SelectedUser, SelectedTeam));

            }else if(p is UserWithRolesDto user)
            {
                SelectedUser = user;
                SelectedTeam = null;
                UserTeamStore?.SelectUserTeam(new UserTeamDto(SelectedUser, SelectedTeam));

            }
        }

        private void SelectUserTeam(object? p)
        {
            if (p is null)
                return;

            var Wind = (SelectUserTeamWindow)p;
            if(SelectedUser is null && SelectedTeam is null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un utilisateur ou une équipe pour continuer."));
                return;
                
            }
            Wind.Close();
        }

        private void GenerateUsers()
        {
            //Users = new ObservableCollection<UserWithRolesDto>
            //{
            //    new UserWithRolesDto(Guid.Parse("9199CE73-0AEB-4A69-9D33-1BBB5CE36A38"), "John Doe", "John Doe - Admin", new List<RoleDto> { new RoleDto(Guid.NewGuid(), "Admin", true) }, "Admin"),
            //    new UserWithRolesDto(Guid.Parse("B3D8290F-FBE8-4E83-BEB6-91932594774B"), "Jane Doe", "Jane Doe - User", new List<RoleDto> { new RoleDto(Guid.NewGuid(), "User", true) }, "User"),
            //    new UserWithRolesDto(Guid.Parse("53510025-106F-43DE-A759-EF4A94F0013A"), "John Smith", "John Smith - User", new List<RoleDto> { new RoleDto(Guid.NewGuid(), "User", true) }, "User"),
            //    new UserWithRolesDto(Guid.Parse("06D0E2E3-97FF-4645-AE55-5AD3BBDCCB52"), "Jane Smith", "Jane Smith - User", new List<RoleDto> { new RoleDto(Guid.NewGuid(), "User", true) }, "User"),

            //};

        }
      

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            SearchEquipmentHelper searchHelper = new();


            if (string.IsNullOrEmpty(SearchText) || Users is null || Users.Count <= 0)
            {
                // Make filtered list the same
                FilterUsers = new ObservableCollection<UserWithRolesDto>(Users ?? Enumerable.Empty<UserWithRolesDto>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterUsers = new ObservableCollection<UserWithRolesDto>(Users.Where(s => s.UserDto.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

        private void TeamSearch()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mTeamLastSearchText) && string.IsNullOrEmpty(TeamSearchText)) ||
                string.Equals(mTeamLastSearchText, TeamSearchText))
                return;

            SearchEquipmentHelper searchHelper = new();


            if (string.IsNullOrEmpty(SearchText) || Teams is null || Teams.Count <= 0)
            {
                // Make filtered list the same
                FilterTeams = new ObservableCollection<TeamDto>(Teams ?? Enumerable.Empty<TeamDto>());

                // Set last search text
                mTeamLastSearchText = TeamSearchText;

                return;
            }
            FilterTeams = new ObservableCollection<TeamDto>(Teams.Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mTeamLastSearchText = TeamSearchText;

        }
    }
}
