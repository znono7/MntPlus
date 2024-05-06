using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {
        public AssetTypes AssetTypes { get; set; }
        public AssetType? SelectedAssetType { get; set; }

        public ObservableCollection<LocationDto> LocationDtos { get; set; }
        public LocationDto? SelectedLocation { get; set; }

        public AssetStatuses AssetStatuses { get; set; }
        public AssetStatus? SelectedAssetStatus { get; set; }


        private DateTime? _dueDate ;
        public DateTime? AssetCommissionDate { get => _dueDate; set => _dueDate = value; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("d");


       
        public AssetDto Asset { get; set; }
        public AssetStore EquipmentStore { get; }
       
        public bool DatePopup { get; set; }
      
        public bool DimmableOverlayVisible =>  DatePopup ;
       
        public ICommand UpdateCommand { get; set; }
      
        public ICommand CommissionDateCommand { get; set; }
        public ICommand CloseCommissionDateCommand { get; set; }
       
        public ICommand PopupClickawayCommand { get; set; }

        public ICommand AddLocationCommand { get; set; }

       public LocationStore? LocationStore { get; set; }
        public EquipmentInfoViewModel(AssetDto equipment , AssetStore equipmentStore)
        {
            LoadLocations().GetAwaiter().GetResult();
            AssetTypes = new AssetTypes();
            AssetStatuses = new AssetStatuses();
            //LoadDataAsync().GetAwaiter().GetResult();
            Asset = equipment;
            SelectedAssetType = new AssetType();
            if(equipment.Type is not null)
                SelectedAssetType = AssetTypes.AssetTypeList.FirstOrDefault(x => x.Name == equipment.Type);
            SelectedAssetStatus = new AssetStatus();
            if(equipment.Status is not null)
                SelectedAssetStatus = AssetStatuses.AssetStatusList.FirstOrDefault(x => x.Name == equipment.Status);

            _dueDate = equipment.AssetCommissionDate;

            if(equipment.Location is not null && LocationDtos is not null)
            {
                SelectedLocation = LocationDtos.FirstOrDefault(x => x.Id == equipment.Location.Id);
            }
            
            EquipmentStore = equipmentStore;
           
            CommissionDateCommand = new RelayCommand(() => DatePopup = !DatePopup);
            CloseCommissionDateCommand = new RelayCommand(() => DatePopup = false);

            UpdateCommand = new RelayCommand(async () => await UpdateAsset());
            LocationStore = new LocationStore();
            LocationStore.LocationCreated += LocationStore_LocationCreated;
            AddLocationCommand = new RelayCommand(ShowLocationWindow);


        }

        private void ShowLocationWindow()
        {
            LocationWindowViewModel locationWindowViewModel = new (LocationStore);
            LocationWindow locationWindow = new LocationWindow();
            locationWindow.DataContext = locationWindowViewModel;
            locationWindow.ShowDialog();
        }

        private void LocationStore_LocationCreated(LocationDto? dto)
        {
            LocationDtos.Add(dto!);
        }

        //load Locations From db

        private async Task LoadLocations()
        {
            var loc = await AppServices.ServiceManager.LocationService.GetAllLocationsAsync(false);
            if(loc is ApiNotFoundResponse)
            {
                LocationDtos = new ObservableCollection<LocationDto>();

            }else if(loc is not null && loc is ApiOkResponse<IEnumerable<LocationDto>> okResponse)
            {
                LocationDtos = new ObservableCollection<LocationDto>(okResponse.Result!);
            }
            else
            {
                LocationDtos = new ObservableCollection<LocationDto>();
            }
        }

       
      
        private async Task UpdateAsset()
        {
           var modelForUpdate = new AssetForCreationDto
           (
                AssetParent : Asset.AssetParent,
                Name: Asset.Name,
                Description: Asset.Description,
                Status : SelectedAssetStatus?.Name,
                Type: SelectedAssetType?.Name,
                LocationId : SelectedLocation?.Id,
                SerialNumber: Asset.SerialNumber,
                Model: Asset.Model,
                Make: Asset.Make,
                PurchaseCost: Asset.PurchaseCost,
                ImagePath: Asset.ImagePath,
                AssetImage: Asset.AssetImage,
                AssetCommissionDate : AssetCommissionDate
            );
            var response = await AppServices.ServiceManager.AssetService.UpdateAsset(Asset.Id, modelForUpdate, true);
            if(response is ApiOkResponse<AssetDto> okResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Asset Updated Successfully"));
                EquipmentStore.UpdateAsset(okResponse.Result!);
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Failed to Update Asset"));
            }
        }
        //private async Task LoadDataAsync()
        //{
        //    await Task.WhenAll(
        //        LoadAssignee(),
        //        LoadDepartments(),
        //        LoadClasses(),
        //        LoadOrganizations(),
        //        LoadEquipmentTypes(),
        //        LoadEquipmentStatuses()
        //    );
        //}
       

    }
}
