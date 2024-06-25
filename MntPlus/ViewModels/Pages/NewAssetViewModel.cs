using Entities;
using Shared;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class NewAssetViewModel : BaseViewModel
    {
        public string Title => IsForUpdated ? "Modifier l'équipement" : "Nouvel équipement";
        public string SaveButtonContent => IsForUpdated ? "Modifier" : "Enregistrer";
        private AssetDto? _asset { get; set; }
        public AssetDto? Asset { get => _asset; set { _asset = value; MapDtoToViewModel(value!); } }
        public bool IsForUpdated { get;  set; }
        public Guid? AssetParent { get; set; }
        public AssetDto? Parent { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AssetStatuses? AssetStatuses { get; set; }
        public AssetStatus? SelectedAssetStatus { get; set; }
        public string? Status { get; set; }

        public EquipmentCategories? EquipmentCategories { get; set; }
        public EquipmentCategory? SelectedEquipmentCategory { get; set; }
        public string? Category { get; set; }

        public Guid? LocationId { get; set; }
        public LocationDto? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public double? PurchaseCost { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? AssetImage { get; set; }
        public DateTime? AssetCommissionDate { get; set; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("d");
        public DateTime? CreatedDate => DateTime.Now;
        public DateTime? PurchaseDate { get; set; }
        public string? ShortPurchaseDate => PurchaseDate?.ToString("d");

        public bool IsMenuDueDateOpen { get; set; }
        public bool IsMenuPurchaseDateOpen { get; set; }

        public ICommand OpenMenuPurchaseDateCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public BitmapImage? MyImageSource { get; set; }
        public bool IsHaveImage { get; set; }
        public ICommand DeleteImgCommand { get; set; }
        public AssetStore AssetStore { get; set; }

        public ICommand SaveCommand { get; set; }
        public bool SaveIsRunning { get; set; }
        public ICommand SelectParentCommand { get; set; }
        public AssetStore SelectedAssetStore { get; private set; }
        public bool IsHaveParent { get; set; }
        public ICommand ClearParentCommand { get; set; }

        public ICommand SelectEquipmentLocationCommand { get; set; }
        public LocationStore? LocationStore { get; set; }
        public bool IsHaveLocation { get; set; }
        public ICommand ClearLocationCommand { get; set; }


        public ICommand BackPageCommand { get; set; }

        public NewAssetViewModel()
        {
            BackPageCommand = new RelayCommand(BackPage);
            AssetStatuses = new AssetStatuses();
            EquipmentCategories = new EquipmentCategories();
            OpenMenuPurchaseDateCommand = new RelayCommand(() => IsMenuPurchaseDateOpen = !IsMenuPurchaseDateOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            BrowseCommand = new RelayCommand(async () => await Browse());
            DeleteImgCommand = new RelayCommand(RemoveImage);
            SaveCommand = new RelayCommand(async () => await Save());
            SelectParentCommand = new RelayCommand(SelectParent);
            ClearParentCommand = new RelayCommand(ClearParent);
            ClearLocationCommand = new RelayCommand(ClearLocation);
            SelectEquipmentLocationCommand = new RelayCommand(SelectLocation);
        }

        private void BackPage()
        {
            IoContainer.Application.GoToPage(ApplicationPage.Assets);
        }
        private void ClearLocation()
        {
            Location = null;
            IsHaveLocation = false;
        }

        private void SelectLocation()
        {
            LocationStore = new LocationStore();
            LocationStore.LocationSelected += OnSelectedLocation;
            SelectEquipmentLocationWindow locationWindow = new() { DataContext = new SelectEquipmentLocationViewModel(LocationStore) };
            locationWindow.ShowDialog();
        }

        private void OnSelectedLocation(LocationDto? dto)
        {
            if (dto != null)
            {
                Location = dto;
                LocationId = dto.Id;
                IsHaveLocation = true;
            }
        }

        private void ClearParent()
        {

            Parent = null;
            AssetParent = null;
            IsHaveParent = false;

        }

        private void SelectParent() 
        {
            SelectedAssetStore = new AssetStore();
            SelectedAssetStore.AssetCreated += OnSelectedEquipment;
            SelectEquipmentWindow selectEquipmentWindow = new SelectEquipmentWindow() { DataContext = new SelectEquipmentViewModel(SelectedAssetStore) };
            selectEquipmentWindow.ShowDialog();
        }

        private void OnSelectedEquipment(AssetDto? dto)
        {
            if (dto != null)
            {
                Parent = dto;
                AssetParent = dto.Id;
                IsHaveParent = true;
            }
        }

        private async Task Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Warning, "Le Nom est requis"));

                return;
            }
            if(IsForUpdated)
            {
                await UpdateAsset();
                return;
            }
            
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var asset = new AssetForCreationDto
                (
                    AssetParent: AssetParent,
                    Name: Name,
                    Description: Description,
                    Status: SelectedAssetStatus?.Name,
                    Category: SelectedEquipmentCategory?.Name,
                    LocationId: LocationId,
                    SerialNumber: SerialNumber,
                    Model: Model,
                    Make: Make,
                    PurchaseCost: PurchaseCost,
                    ImagePath: ImagePath,
                    AssetImage: AssetImage,
                    AssetCommissionDate: AssetCommissionDate,
                    CreatedDate: CreatedDate,
                    PurchaseDate: PurchaseDate

                );
                var Response = await AppServices.ServiceManager.AssetService.CreateAsset(asset);
                if (Response.Success && Response is ApiOkResponse<AssetDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Équipement créé avec succès"));
                    AssetStore.CreateAsset(result.Result);
                   BackPage();
                }
                else if (Response is ApiBadRequestResponse apiBad)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, $"Erreur {apiBad.Message}"));
                }

            });
        }

        private async Task UpdateAsset()
        {
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var asset = new AssetForCreationDto
                (AssetParent: AssetParent,
                 Name: Name,
                 Description: Description,
                 Status: SelectedAssetStatus?.Name,
                 Category: SelectedEquipmentCategory?.Name,
                 LocationId: LocationId,
                 SerialNumber: SerialNumber,
                 Model: Model,
                 Make: Make,
                 PurchaseCost: PurchaseCost,
                 ImagePath: ImagePath,
                 AssetImage: AssetImage,
                 AssetCommissionDate: AssetCommissionDate,
                 CreatedDate: CreatedDate,
                 PurchaseDate: PurchaseDate);
                var Response = await AppServices.ServiceManager.AssetService.UpdateAsset(Asset!.Id, asset,true);
                if (Response.Success && Response is ApiOkResponse<AssetDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Équipement mis à jour avec succès"));
                    AssetStore.UpdateAsset(result.Result);
                    BackPage();
                }
                else if (Response is ApiBadRequestResponse apiBad)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, $"Erreur {apiBad.Message}"));
                }

            });
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
                AssetImage = File.ReadAllBytes(selectedFilePath);
                IsHaveImage = true;
            }
            await Task.Delay(1);
        }
        private void RemoveImage()
        {
            ImagePath = null;
            MyImageSource = null;
            AssetImage = null;
            IsHaveImage = false;
        }

        private void MapDtoToViewModel(AssetDto dto)
        {
            IsForUpdated = true;
            AssetParent = dto.AssetParent;
            Name = dto.Name;
            Description = dto.Description;
            Status = dto.Status;
            Category = dto.Category;
            LocationId = dto.LocationId;
            Location = dto.Location;
            SerialNumber = dto.SerialNumber;
            Model = dto.Model;
            Make = dto.Make;
            PurchaseCost = dto.PurchaseCost;
            ImagePath = dto.ImagePath;
            AssetImage = dto.AssetImage;
            AssetCommissionDate = dto.AssetCommissionDate;
            PurchaseDate = dto.PurchaseDate;
        }
        public override void Dispose()
        {
            SelectedAssetStore.AssetCreated += OnSelectedEquipment;
            base.Dispose();

        }
    }
}
