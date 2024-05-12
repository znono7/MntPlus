﻿using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class UsersPageViewModel : BaseViewModel
    {
        public ObservableCollection<UserDto>? FilterUsers { get; set; }
        private ObservableCollection<UserDto>? users { get; set; }
        public ObservableCollection<UserDto>? Users
        { 
            get => users; 
            set {
                users = value; 
                if (users is not null)
                    FilterUsers = new ObservableCollection<UserDto>(users);
            } }
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
        public UsersPageViewModel()
        {
            MenuActionCommand = new RelayCommand(() => IsMenuActionOpen = !IsMenuActionOpen);
            ToUsersPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Users));
            ToTeamsPageCommand = new RelayCommand(async () => await NavigateToPageAsync(ApplicationPage.Teams));
            OpenWindowCommand = new RelayCommand(OpenUserWindow);
            DeleteUsersCommand = new RelayCommand(RemoveSelectedItems);
            SearchCommand = new RelayCommand(Search);

        }

        private void OpenUserWindow()
        {
            AddUserWindow addUserWindow = new () { DataContext = new AddUserViewModel()};
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
            var selectedItems = FilterUsers.Where(item => item.IsChecked).ToList();
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
                FilterUsers = new ObservableCollection<UserDto>(Users ?? Enumerable.Empty<UserDto>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterUsers = new ObservableCollection<UserDto>(
                Users.Where(item => item.FirstName is not null && item.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
               item.LastName is not null && item.FirstName is not null && item.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

    }
}
