using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class LocationsPageViewModel : BaseViewModel
    {
        public LocationWindowViewModel locationWindow { get; set; }
        public bool AddLocationPopupIsOpen { get; set; }
        public bool DimmableOverlayVisible { get; set; }
        public ObservableCollection<LocationDto>? LocationDtos { get; set; }
        public ObservableCollection<PrimaryLocationViewModel>? FilterPrimaryLocationViews { get; set; }
        private ObservableCollection<PrimaryLocationViewModel>? primaries { get; set; }
        public ObservableCollection<PrimaryLocationViewModel>? Primaries
        { 
            get => primaries;
            set
            {
                primaries = value;
                if (primaries is not null)
                {
                    FilterPrimaryLocationViews = new ObservableCollection<PrimaryLocationViewModel>(primaries);
                }
            }
        }

        protected string? mLastSearchText;
        protected string? mSearchText;
        public ICommand SearchCommand { get; set; }
        public ICommand OpenWindowCommand { get; set; } 

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

        public LocationStore? LocationStore { get; set; }

        public LocationsPageViewModel()
        {
            _ = GetLocations();
            OrginaseData();
            SearchCommand = new RelayCommand(Search);
            LocationStore = new LocationStore();
            LocationStore.LocationCreated += AddNewLocation;
            locationWindow = new LocationWindowViewModel(LocationStore);
            locationWindow.CloseAction += CloseControl;
            OpenWindowCommand = new RelayCommand(() =>
            {
                AddLocationPopupIsOpen = true;
                DimmableOverlayVisible = true;
            });
        }

        private async Task CloseControl()
        {
            AddLocationPopupIsOpen = false;
            DimmableOverlayVisible = false;
            await Task.Delay(1);
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
                FilterPrimaryLocationViews = new ObservableCollection<PrimaryLocationViewModel>(Primaries ?? Enumerable.Empty<PrimaryLocationViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterPrimaryLocationViews = new ObservableCollection<PrimaryLocationViewModel>(
                Primaries.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

        private async Task GetLocations()
        {
            var response = await AppServices.ServiceManager.LocationService.GetAllLocationsAsync(false);
            if(response != null && response is ApiOkResponse<IEnumerable<LocationDto>> result)
            {
                LocationDtos = new ObservableCollection<LocationDto>(result.Result!);
            }
            else
            {
                LocationDtos = new ObservableCollection<LocationDto>();

            }
        }
        private void OrginaseData()
        {
            if (LocationDtos is null)
                return;
            Primaries = new ObservableCollection<PrimaryLocationViewModel>();
            foreach (var location in LocationDtos)
            {
                if (location.IsPrimaryLocation)
                {
                    Primaries.Add(new PrimaryLocationViewModel(location));
                }
            }

            if (Primaries?.Count > 0)
            {
                foreach (var primary in Primaries)
                {
                    foreach (var location in LocationDtos)
                    {
                        if (!location.IsPrimaryLocation && location.IdParent == primary.Location?.Id)
                        {
                            primary.SubLocations ??= new ObservableCollection<SubLocationViewModel>();
                            primary.SubLocations.Add(new SubLocationViewModel(location));
                        }
                    }
                }
            }

            FilterPrimaryLocationViews = new ObservableCollection<PrimaryLocationViewModel>(Primaries!);
        }

        private void AddNewLocation(LocationDto? location)
        {
            if (location is null)
                return;

            if (location.IsPrimaryLocation && location.IdParent is null)
            {
                Primaries ??= new ObservableCollection<PrimaryLocationViewModel>();
                Primaries.Add(new PrimaryLocationViewModel(location));

            }
            else
            {
                if (Primaries?.Count > 0)
                {
                    foreach (var primary in Primaries)
                    {
                          
                        if (!location.IsPrimaryLocation && location.IdParent == primary.Location?.Id)
                            {
                                primary.SubLocations ??= new ObservableCollection<SubLocationViewModel>();
                                primary.SubLocations.Add(new SubLocationViewModel(location));
                            }
                        
                    }

                }
            }
            FilterPrimaryLocationViews = new ObservableCollection<PrimaryLocationViewModel>(Primaries);



        }

    }
}
