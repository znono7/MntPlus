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
    public class SelectEquipmentLocationViewModel : BaseViewModel
    {
        public ObservableCollection<LocationDto>? LocationDtos { get; set; }

        public LocationItemViewModel? SelectedViewModel { get; set; }
        public ObservableCollection<LocationItemViewModel>? FilterPrimaryLocationViews { get; set; }
        private ObservableCollection<LocationItemViewModel>? primaries { get; set; }
        public ObservableCollection<LocationItemViewModel>? Primaries
        {
            get => primaries;
            set
            {
                primaries = value;
                if (primaries is not null)
                {
                    FilterPrimaryLocationViews = new ObservableCollection<LocationItemViewModel>(primaries);
                }
            }
        }

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

                Search();
            }
        }
        public ICommand GetSelectedLocationCommand { get; set; }
        public ICommand SelecteLocationCommand { get; set; }

        public LocationStore? LocationStore { get; }

        public SelectEquipmentLocationViewModel(LocationStore? locationStore)
        {
            LocationStore = locationStore;
            GetSelectedLocationCommand = new RelayParameterizedCommand( (p) => GetSelectedLocation(p));
            SelecteLocationCommand = new RelayParameterizedCommand(async (p) => await SelectLocation(p));
            //_ = GetLocations();
            LocationDtos = new ObservableCollection<LocationDto>
            {
                new LocationDto(Guid.Parse("7C4D383E-0BB0-4860-9B7C-A83FDD6967C0"),"First Location","Aflou" , true,null,null,DateTime.Now)
            };
            Primaries = new CreateLocationsTree().CreateTreeViewItems(LocationDtos?.ToList());
        }

        private async Task  SelectLocation(object? p)
        {
            if(SelectedViewModel is null)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Warning, "Aucun localisation spécifié"));
                return;
            }
            var window = p as SelectEquipmentLocationWindow;
            if (window != null)
            {
                LocationStore?.SelectLocation(SelectedViewModel.LocationDto);
                window.Close();
            }
        }

        private void GetSelectedLocation(object? p)
        {
            if (p == null) return;
            var dto = p as LocationItemViewModel;
            SelectedViewModel = dto;
        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || Primaries is null || Primaries.Count <= 0)
            {
                // Make filtered list the same
                FilterPrimaryLocationViews = new ObservableCollection<LocationItemViewModel>(Primaries ?? Enumerable.Empty<LocationItemViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterPrimaryLocationViews = new ObservableCollection<LocationItemViewModel>(
                Primaries.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }
    }
}
