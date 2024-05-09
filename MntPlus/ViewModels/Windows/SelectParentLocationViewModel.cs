using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SelectParentLocationViewModel : BaseViewModel
    {
        public PrimarySelectLocationViewModel? SelectedViewModel { get; set; }
        public ObservableCollection<LocationDto>? LocationDtos { get; set; }
        public ObservableCollection<PrimarySelectLocationViewModel>? FilterPrimaryLocationViews { get; set; }
        private ObservableCollection<PrimarySelectLocationViewModel>? primaries { get; set; }
        public ObservableCollection<PrimarySelectLocationViewModel>? Primaries
        {
            get => primaries;
            set
            {
                primaries = value;
                if (primaries is not null)
                {
                    FilterPrimaryLocationViews = new ObservableCollection<PrimarySelectLocationViewModel>(primaries);
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

        public SelectParentLocationViewModel()
        {
            Primaries = new ObservableCollection<PrimarySelectLocationViewModel>();
            GetSelectedLocationCommand = new RelayParameterizedCommand(async(p) => await GetSelectedLocation(p));
        }

        private async Task GetSelectedLocation(object? p)
        {
            var window = p as SelectParentLocationWindow;
            if(SelectedViewModel is  null) 
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un Localisation"));
            }
         
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
                FilterPrimaryLocationViews = new ObservableCollection<PrimarySelectLocationViewModel>(Primaries ?? Enumerable.Empty<PrimarySelectLocationViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterPrimaryLocationViews = new ObservableCollection<PrimarySelectLocationViewModel>(
                Primaries.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

        private void IterateLocations()
        {
            if (LocationDtos is null)
                return;
            foreach (var location in LocationDtos)
            {
                if (location.IsPrimaryLocation && location.IdParent is null)
                {
                    Primaries ??= new ObservableCollection<PrimarySelectLocationViewModel>();

                    PrimarySelectLocationViewModel Vmodel = new(location) { SelectLocationFunc = SetSelectedLocation };


                    Primaries.Add(Vmodel);
                }
            }
        }

        private async Task SetSelectedLocation(PrimarySelectLocationViewModel? model)
        {
            if (model is null)
                return;
            SelectedViewModel = model;
            await Task.Delay(1);
        }

       
    }
}
