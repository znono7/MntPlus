using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class PrimaryLocationViewModel : BaseViewModel
    {
        public LocationDto? Location { get; set; }
        public string? Name { get; set; }
        public string? Adress { get; set; }
        public string? CreatedAt { get; set; }  //=> Location?.CreatedAt.ToShortDateString();

        public bool IsExpanded { get; set; }
        public ICommand? ExpandCommand { get; set; }
        public ObservableCollection<SubLocationViewModel>? SubLocations { get; set; }
        public bool IsHaveSubLocations => SubLocations?.Count > 0;

       
        public PrimaryLocationViewModel(LocationDto? location)
        {
            Location = location;
            Name = location?.Name;
            Adress = location?.Address;
            CreatedAt = location?.CreatedAt.ToShortDateString();
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
