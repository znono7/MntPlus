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
    public class TeamsPageViewModel : BaseViewModel
    {
        public ObservableCollection<TeamDto>? FilterTeams { get; set; }
        private ObservableCollection<TeamDto>? teams { get; set; }
        public ObservableCollection<TeamDto>? Teams
        {
            get => teams;
            set
            {
                teams = value;
                if (teams is not null)
                    FilterTeams = new ObservableCollection<TeamDto>(teams);
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
        public ICommand DeleteTeamsCommand { get; set; }
        public TeamsPageViewModel()
        {
            MenuActionCommand = new RelayCommand(() => IsMenuActionOpen = !IsMenuActionOpen);
            ToUsersPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Users));
            ToTeamsPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Teams));
            OpenWindowCommand = new RelayCommand(OpenTeamWindow);
            DeleteTeamsCommand = new RelayCommand(RemoveSelectedItems);
            SearchCommand = new RelayCommand(Search);

        }
        private void OpenTeamWindow()
        {
            AddTeamWindow addUserWindow = new() { DataContext = new AddTeamViewModel() };
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
            //if (FilterTeams is null || FilterTeams.Count == 0)
            //    return;
            //var selectedItems = FilterTeams.Where(item => item.IsChecked).ToList();
            //foreach (var item in selectedItems)
            //{
            //    FilterTeams.Remove(item);
            //}
        }
        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || Teams is null || Teams.Count <= 0)
            {
                // Make filtered list the same
                FilterTeams = new ObservableCollection<TeamDto>(Teams ?? Enumerable.Empty<TeamDto>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterTeams = new ObservableCollection<TeamDto>(
                Teams.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

    }
}
 