using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class LocationWindowViewModel : BaseViewModel
    {

        public string? Name { get; set; }
        public string? Address { get; set; }
       
        public LocationDto? ParentLocation { get; set; }
        public string? ParentName { get; set; }


        public ICommand AddCommand { get; set; }
        public ICommand BrowseToParentCommand { get; set; }
        public LocationStore? LocationStore { get; }
        public bool SaveIsRunning { get; set; }
        public bool browseVisible { get; set; }= true;

        public LocationWindowViewModel(LocationStore? locationStore = null ) 
        {
            AddCommand = new RelayParameterizedCommand(async (p) => await AddLocation(p));
            LocationStore = locationStore;
        }

        private async Task AddLocation(object? p)
        {
            if (p is not Window window)
                return;
                

            //var location = new LocationForCreationDto
            //(
            //    Name : Name,
            //    Address : Address
               
            //);
            //var response = await AppServices.ServiceManager.LocationService.CreateLocation(location);
            //if(response is ApiOkResponse<LocationDto> result)
            //{
            //    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success,"Location added successfully"));
            //   if(LocationStore is not null )
            //    {
            //        LocationStore.CreateLocation(result.Result);
            //    }
            //    window.Close();
            //}
            //else
            //{
            //    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error,"Failed to add location"));

            //}
        }

      
    }
}
