using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class PrimaryLocationViewModel : BaseViewModel
    {
        public LocationDto? Location { get; set; }
        public string? Name => Location?.Name;
        public string? Adress => Location?.Address;
        public string? CreatedAt => Location?.CreatedAt.ToShortDateString();

        public bool IsExpanded { get; set; }
        public ICommand? ExpandCommand { get; set; }
        public ObservableCollection<SubLocationViewModel>? SubLocations { get; set; }
        public bool IsHaveSubLocations => SubLocations?.Count > 0;

       
        public PrimaryLocationViewModel(LocationDto? location)
        {
            Location = location;
            SubLocations = new ObservableCollection<SubLocationViewModel>();
            ExpandCommand = new RelayCommand(() => IsExpanded = !IsExpanded);
        }
    }

    public class PrimarySelectLocationViewModel : PrimaryLocationViewModel
    {
        public ICommand SelectLocationCommand { get; set; }
        public Func<PrimarySelectLocationViewModel?, Task>? SelectLocationFunc { get; set; }
        public PrimarySelectLocationViewModel(LocationDto? location) : base(location)
        {
            SelectLocationCommand = new RelayCommand(async () => await SelectLocation());

        } 

        private async Task SelectLocation()
        {

            if (SelectLocationFunc is not null)
            {
                await SelectLocationFunc(this);
            }
        }
    }
}
