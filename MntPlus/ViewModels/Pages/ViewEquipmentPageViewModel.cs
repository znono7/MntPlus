using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input; 

namespace MntPlus.WPF
{
    public class ViewEquipmentPageViewModel : BaseViewModel
    {
        private AssetDto? _asset { get; set; }
        public AssetDto? asset 
        {
            get => _asset; 
            set
            {  
                if (value == null)
                    return;
                _asset = value;
                equipmentInfo = new EquipmentInfoViewModel(value);
                equipmentPartViewModel = new EquipmentPartViewModel(value);
                equipmentMeterViewModel = new EquipmentMeterViewModel(value.Id);
                
                OnPropertyChanged(nameof(asset));
            }
        }

        private EquipmentItemViewModel? _equipmentItem { get; set; }
        public EquipmentItemViewModel? equipmentItem
        {
            get => _equipmentItem;
            set
            {
                if (value == null)
                    return;
                _equipmentItem = value;
                childrenViewModel = new AssetChildrenViewModel(value);
                OnPropertyChanged(nameof(equipmentItem));
            }
        }
        public Guid EquipmentId { get; set; }
        public AssetStore? AssetStore { get; set; }
        public EquipmentInfoViewModel equipmentInfo { get; set;}
        public AssetChildrenViewModel childrenViewModel { get; set; }
        public EquipmentPartViewModel equipmentPartViewModel { get; set; }
        public EquipmentMeterViewModel equipmentMeterViewModel { get; set; }
        public ICommand BackPageCommand { get; set; }
        public ICommand EditEquipmentCommand { get; set; }
        public ICommand DeleteEquipmentCommand { get; set; }

        public ViewEquipmentPageViewModel()
        { 
            BackPageCommand = new RelayCommand(BackPage);
            EditEquipmentCommand = new RelayCommand(EditWorkOrder);
            DeleteEquipmentCommand = new RelayCommand(async () => await DeleteEquipment());
        }

        private async Task DeleteEquipment()
        {
            var Dialog = new ConfirmationWindow("Supprimé Equipement");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
            {
                return;
            }
                var response = await AppServices.ServiceManager.AssetService.DeleteAsset(asset!.Id, true);
            if (response is ApiOkResponse<AssetDto> && response.Success)
            {
               
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipement Supprimé avec Succès"));
                AssetStore?.DeleteAsset(asset);
                BackPage();
            }
            else if (response is ApiBadRequestResponse errorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.Message));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'équipement"));
            }
        }

        private void  EditWorkOrder()
        {
            IoContainer.Application.GoToPage(ApplicationPage.NewAsset, new NewAssetViewModel { Asset = asset , AssetStore = AssetStore!});
        }

        private void BackPage() 
        {
            IoContainer.Application.GoToPage(ApplicationPage.Assets);
            
        }
    }
}
