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

namespace MntPlus.WPF
{
    public class EquipmentDataViewModel : BaseViewModel
    {

        public EquipmentInfoViewModel? EquipmentInfoViewModel { get; set; }
        public EquipmentDto Equipment { get; set; }
        public EquipmentStore EquipmentStore { get; }
        public string? EquipmentName { get; set; }
        public string? EquipmentImage { get; private set; }
        public BitmapImage? MyImageSource { get; private set; }
        public byte[]? EquipmentImageBytes { get; private set; }
        public bool IsHaveImage { get; set; }

        public ICommand BrowseCommand { get; set; }
        public ICommand DeleteImgCommand { get; set; }

        public EquipmentDataViewModel(EquipmentDto equipment , EquipmentStore equipmentStore)
        {
            Equipment = equipment;
            EquipmentStore = equipmentStore;
            EquipmentName = equipment.EquipmentName;
            EquipmentImageBytes = equipment.EquipmentImage;
            ReadImage();
            BrowseCommand = new RelayCommand(async () => await Browse());
            EquipmentInfoViewModel = new EquipmentInfoViewModel(equipment, equipmentStore);
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

                if (EquipmentImageBytes != Equipment.EquipmentImage && EquipmentImageBytes.Length > 0)
                {
                    var Result = await AppServices.ServiceManager.EquipmentService.UpdateEquipmentImageAsync(Equipment.Id, new EquipmentForImageUpdateDto(EquipmentImage, EquipmentImageBytes), true);
                    if (Result is not null && Result is EquipmentDeleteErrorResponse)
                    {
                        IsHaveImage = true;
                        await IoContainer.NotificationsManager.ShowMessage( new NotificationControlViewModel(NotificationType.Error,"Error updating equipment image"));
                    }else if (Result is not null && Result is ApiOkResponse<Equipment> response)
                    {
                        Equipment = MapToEquipmentDTO(response.Result)!;
                        EquipmentStore.UpdateEquipment(Equipment);
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
            var Result = await AppServices.ServiceManager.EquipmentService.UpdateEquipmentImageAsync(Equipment.Id, new EquipmentForImageUpdateDto(EquipmentImage, null), true);
            if (Result is not null && Result is EquipmentDeleteErrorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
            else if (Result is not null && Result is ApiOkResponse<Equipment> response)
            {
                
                //Equipment = MapToEquipmentDTO(response.Result)!;
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipment image updated successfully"));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
        }

        private EquipmentDto? MapToEquipmentDTO(Equipment? equipment)
        {
           
            return new EquipmentDto
            (
                Id: equipment.Id,
                EquipmentParent: equipment.EquipmentParent,
                EquipmentName: equipment.EquipmentName,
                EquipmentType: equipment.EquipmentType is null ? null : new EquipmentTypeDto(Id: equipment.EquipmentType.Id, EquipmentTypeName: equipment.EquipmentType.EquipmentTypeName),
                EquipmentDescription: equipment.EquipmentDescription,
                EquipmentOrganization: equipment.EquipmentOrganization is null ? null : new EquipmentOrganizationDto(Id: equipment.EquipmentOrganization.Id, OrganizationName: equipment.EquipmentOrganization.OrganizationName),
                EquipmentDepartment: equipment.EquipmentDepartment is null ? null : new EquipmentDepartmentDto(Id: equipment.EquipmentDepartment.Id, DepartmentName: equipment.EquipmentDepartment.DepartmentName),
                EquipmentClass: equipment.EquipmentClass is null ? null : new EquipmentClassDto(Id: equipment.EquipmentClass.Id, ClassName: equipment.EquipmentClass.EquipmentClassName),
                EquipmentSite: equipment.EquipmentSite,
                EquipmentStatus: equipment.EquipmentStatus is null ? null : new EquipmentStatusDto(Id: equipment.EquipmentStatus.Id, StatusName: equipment.EquipmentStatus.EquipmentStatusName),
                EquipmentMake: equipment.EquipmentMake,
                EquipmentSerialNumber: equipment.EquipmentSerialNumber,
                EquipmentModel: equipment.EquipmentModel,
                EquipmentCost: equipment.EquipmentCost,
                EquipmentCommissionDate: equipment.EquipmentCommissionDate,
                EquipmentAssignedTo: equipment.EquipmentAssignedTo is null ? null : new EquipmentAssignedToDto(Id: equipment.EquipmentAssignedTo.Id, AssignedToName: equipment.EquipmentAssignedTo.Name),
                EquipmentNameImage: equipment.EquipmentNameImage,
                EquipmentImage: equipment.EquipmentImage
            );
        }
    }
}
 