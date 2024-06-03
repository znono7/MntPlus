using Entities;
using Shared;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class LocationWindowViewModel : BaseViewModel
    {

        public string? Name { get; set; }
        public string? Address { get; set; }
       
        public LocationDto? ParentLocation { get; set; }
        public string? ParentName { get; set; }


        public ICommand SaveCommand { get; set; }
        public ICommand BrowseToParentCommand { get; set; }
        public LocationStore? LocationStore { get; }
        public LocationStore? LocationParentStore { get; set; }
        public bool SaveIsRunning { get; set; }
        public bool browseToLocationVisible { get; set; }= true;

        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }

        public ICommand ClearLocationParentCommand { get; set; }

        public LocationWindowViewModel(LocationStore? locationStore) 
        {
            SaveCommand = new RelayCommand(async () => await AddLocation());
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            LocationStore = locationStore;
            ClearLocationParentCommand = new RelayCommand(() => { ParentLocation = null;browseToLocationVisible = true; });
           
            BrowseToParentCommand = new RelayCommand(ParentBrowse);
        }

        private void ParentBrowse()
        {
            LocationParentStore = new LocationStore();
            LocationParentStore.LocationSelected += LocationParentStore_LocationSelected;
            SelectParentLocationWindow selectParent = new() { DataContext = new SelectParentLocationViewModel(LocationParentStore) };
            selectParent.ShowDialog();
        }

        private void LocationParentStore_LocationSelected(LocationDto? dto)
        {
            if(dto is not null)
            {
                ParentLocation = dto;
                ParentName = dto.Name;
                browseToLocationVisible = false;
            }
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();

        }
        private async Task AddLocation()
        {
           

            if (string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ""));
                return;
            }
            var location = new LocationForCreationDto
            (
                Name: Name,
                Address: Address ,
                ParentLocation == null ? true : false,
                ParentLocation == null ? null : ParentLocation?.Id,
                DateTime.Now
            );
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var response = await AppServices.ServiceManager.LocationService.CreateLocation(location);
                if (response is ApiOkResponse<LocationDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Localisation ajouté avec succès"));
                    if (LocationStore is not null)
                    {
                        LocationStore.CreateLocation(result.Result);
                    }
                    CloseCommand.Execute(null);
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Échec de l'ajout de localisation"));

                }
            });
           
        }

      
    }
}
