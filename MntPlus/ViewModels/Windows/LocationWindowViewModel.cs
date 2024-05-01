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
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? LocationImage { get; set; }

        public BitmapImage? MyImageSource { get; private set; }
        public bool IsHaveImage { get; set; }

        private bool _DimmableOverlayVisible { get; set; }

        public bool DimmableOverlayVisible { get => _DimmableOverlayVisible; set { _DimmableOverlayVisible = value; OnPropertyChanged(nameof(DimmableOverlayVisible)); } }

        public bool IsBtnEnabled => !DimmableOverlayVisible;

        public ICommand BrowseCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public LocationStore? LocationStore { get; }

        public LocationWindowViewModel(LocationStore? locationStore = null )
        {
            BrowseCommand = new RelayCommand(async () => await Browse());
            AddCommand = new RelayParameterizedCommand(async (p) => await AddLocation(p));
            LocationStore = locationStore;
        }

        private async Task AddLocation(object? p)
        {
            if (p is not Window window)
                return;
                
            DimmableOverlayVisible = true;

            var location = new LocationForCreationDto
            (
                Name : Name,
                Address : Address,
                City : City,
                State : State,
                Country : Country,
                ImagePath : ImagePath,
                LocationImage : LocationImage
            );
            var response = await AppServices.ServiceManager.LocationService.CreateLocation(location);
            if(response is ApiOkResponse<LocationDto> result)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success,"Location added successfully"));
               if(LocationStore is not null )
                {
                    LocationStore.CreateLocation(result.Result);
                }
                window.Close();
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error,"Failed to add location"));

            }
            DimmableOverlayVisible = false;
        }

        public async Task Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";


            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
                ImagePath = Path.GetFileName(selectedFilePath);
                MyImageSource = new BitmapImage(new Uri(selectedFilePath));
                LocationImage = File.ReadAllBytes(selectedFilePath);

               

            }
            await Task.Delay(1);
        }

    }
}
