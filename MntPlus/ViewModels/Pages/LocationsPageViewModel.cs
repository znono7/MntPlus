using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class LocationsPageViewModel : BaseViewModel
    {
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
            SearchCommand = new RelayCommand(Search);
            Primaries = new ObservableCollection<PrimaryLocationViewModel>();
            LocationStore = new LocationStore();
            LocationStore.LocationCreated += AddNewLocation;
            OpenWindowCommand = new RelayCommand(() =>
            {
                var window = new LocationWindow();
                window.DataContext = new LocationWindowViewModel(LocationStore);
                window.ShowDialog();
            });
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

        private void OrginaseData()
        {
            if (LocationDtos is null)
                return;
            foreach (var location in LocationDtos)
            {
                if (location.IsPrimaryLocation && location.IdParent is null)
                {
                    Primaries ??= new ObservableCollection<PrimaryLocationViewModel>();
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



        }

    }
}
