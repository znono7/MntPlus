using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;
using Entities;
using System.Collections.ObjectModel;

namespace MntPlus.WPF
{
    public class EquipmentDataViewModel : BaseViewModel
    {

        public EquipmentInfoViewModel? EquipmentInfoViewModel { get; set; }
        public AssetChildrenViewModel? EquipmentChildrenViewModel { get; set; }
        public AssetDto Equipment { get; set; }
        public AssetStore EquipmentStore { get; }
        public ObservableCollection<AssetItemViewModel>? Children { get; }
        public string? EquipmentName { get; set; }
        public string? EquipmentImage { get; private set; }
        public BitmapImage? MyImageSource { get; private set; }
        public byte[]? EquipmentImageBytes { get; private set; }
        public bool IsHaveImage { get; set; }

        public ICommand BrowseCommand { get; set; }
        public ICommand DeleteImgCommand { get; set; }

        public EquipmentDataViewModel(AssetDto equipment , AssetStore equipmentStore , ObservableCollection<AssetItemViewModel>? _children = null)
        {
            Equipment = equipment;
            EquipmentStore = equipmentStore;
            Children = _children;
            EquipmentName = equipment.Name;
            EquipmentImageBytes = equipment.AssetImage;
            ReadImage();
            BrowseCommand = new RelayCommand(async () => await Browse());
            EquipmentInfoViewModel = new EquipmentInfoViewModel(equipment, equipmentStore);
            EquipmentChildrenViewModel = new AssetChildrenViewModel(equipment.Id, equipmentStore,Children);
            DeleteImgCommand = new RelayCommand(async () => await RemoveImage());

        }


        public void ReadImage()
        {
            if (EquipmentImageBytes != null && EquipmentImageBytes.Length > 0)
            {
               IsHaveImage = true;
                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(EquipmentImageBytes))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                MyImageSource = image;
            }
        }
        public async Task Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";


            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
                EquipmentImage = Path.GetFileName(selectedFilePath);
                MyImageSource = new BitmapImage(new Uri(selectedFilePath));
                EquipmentImageBytes = File.ReadAllBytes(selectedFilePath);

                if (EquipmentImageBytes != Equipment.AssetImage && EquipmentImageBytes.Length > 0)
                {
                    var Result = await AppServices.ServiceManager.AssetService.UpdateAssetImage(Equipment.Id, new AssetForUpdateImage(EquipmentImage, EquipmentImageBytes), true);
                    if (Result is not null && Result is ApiBadRequestResponse)
                    {
                        IsHaveImage = true;
                        await IoContainer.NotificationsManager.ShowMessage( new NotificationControlViewModel(NotificationType.Error,"Error updating equipment image"));
                    }else if (Result is not null && Result is ApiOkResponse<AssetDto> response)
                    {
                        Equipment = response.Result!;
                        EquipmentStore.UpdateAsset(Equipment);
                        await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipment image updated successfully"));
                    }else
                    {
                        await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
                    }
                    

                }

            }
           
        }

        private async Task RemoveImage()
        {
            MyImageSource = null;
            EquipmentImage = null;
            IsHaveImage = false;
            var Result = await AppServices.ServiceManager.AssetService.UpdateAssetImage(Equipment.Id, new AssetForUpdateImage(EquipmentImage, null), true);
            if (Result is not null && Result is ApiBadRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
            else if (Result is not null && Result is ApiOkResponse<AssetDto> response)
            {
                
                //Equipment = MapToEquipmentDTO(response.Result)!;
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipment image updated successfully"));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
        }

    }
}
 