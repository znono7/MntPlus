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
    public class AddMeterViewModel : BaseViewModel
    {
        public Guid MeterId { get; set; }
        public string Name { get; set; }
        public int Frequency { get; set; }
        public double CurrentReading { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }
        public List<UnitOfMeasurementModel> UnitOfMeasurementModels { get; set; }
        public UnitOfMeasurementModel? SelectedUnitOfMeasurement { get; set; }

        public List<FrequencyUnitModel> FrequencyUnitModels { get; set; }
        public FrequencyUnitModel? SelectedFrequencyUnit { get; set; }
        public MeterStore? MeterStore { get; set; }
        public bool IsForUpdate { get; set; } = false;

        public bool browseToEquipmentVisible { get; set; } = true;
        public ICommand BrowseToEquipmentCommand { get; set; }
        public ICommand ClearEquipmentCommand { get; set; }
        public string TitleControl { get;  set; }
        public string? SelectedAsset { get; set; }
        public Guid? SelectedAssetId { get; set; }
        public AssetStore SelectedAssetStore { get;  set; }

        public AddMeterViewModel(MeterStore? meterStore)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            UnitOfMeasurementModels= GetUnitsOfMeasurement();
            FrequencyUnitModels = GetFrequencyUnits();
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            MeterStore = meterStore;
            BrowseToEquipmentCommand = new RelayCommand(BrowseToEquipment);
            ClearEquipmentCommand = new RelayCommand(() => { SelectedAsset = null; SelectedAssetId = null; browseToEquipmentVisible = true; });
            TitleControl = "Ajouter un compteur";

        }

        public AddMeterViewModel(MeterStore? meterStore, MeterDto meterDto)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            UnitOfMeasurementModels = GetUnitsOfMeasurement();
            FrequencyUnitModels = GetFrequencyUnits();
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            MeterStore = meterStore;
            IsForUpdate = true;
            BrowseToEquipmentCommand = new RelayCommand(BrowseToEquipment);
            ClearEquipmentCommand = new RelayCommand(() => { SelectedAsset = null; SelectedAssetId = null; browseToEquipmentVisible = true; });
            SelectedFrequencyUnit = FrequencyUnitModels.FirstOrDefault(x => x.Value == Enum.Parse<FrequencyUnit>(meterDto.FrequencyUnit!));
            SelectedUnitOfMeasurement = UnitOfMeasurementModels.FirstOrDefault(x => x.Value == Enum.Parse<UnitMeasurement>(meterDto.Unit!));
            Name = meterDto.Name!;
            Frequency = meterDto.Frequency;
            SelectedAsset = meterDto.AssetName;
            SelectedAssetId = meterDto.AssetId;
            MeterId = meterDto.Id;
            CurrentReading = meterDto.CurrentReading;
            LastUpdated = meterDto.LastUpdated;
            TitleControl = "Modifier le compteur";

        }

        private void BrowseToEquipment()
        {
            SelectedAssetStore = new AssetStore();
            SelectedAssetStore.AssetCreated += OnSelectedEquipment;
            SelectEquipmentWindow selectEquipmentWindow = new SelectEquipmentWindow() { DataContext = new SelectEquipmentViewModel(SelectedAssetStore) };
            selectEquipmentWindow.ShowDialog();
        }

        private void OnSelectedEquipment(AssetDto? dto)
        {
            SelectedAsset = dto?.Name;
            SelectedAssetId = dto?.Id ?? Guid.Empty;
            browseToEquipmentVisible = false;
        }
        private async Task SaveAsync()
        {
            if(SelectedUnitOfMeasurement == null || SelectedFrequencyUnit == null)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une unité d'unité de mesure et de fréquence"));return;
            }
            if(string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez saisir un nom"));return;
            }
            if(Frequency <= 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La fréquence doit être supérieure à 0"));return;
            } 
            if(SelectedAssetId == null)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un équipement"));return;
            }
            if(IsForUpdate)
            {
                await UpdateMeter();
            }
            else
            {
                await CreateMeter();
            }
          
        }

        private async Task UpdateMeter()
        {
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var meter = new MeterDtoForCreation(Name, CurrentReading, LastUpdated, SelectedUnitOfMeasurement?.Value.ToUnitMeasurementString(), Frequency, SelectedAssetId, SelectedFrequencyUnit?.Value.ToString());
                var response = await AppServices.ServiceManager.MeterService.UpdateMeter(MeterId,meter,true);
                if (response.Success && response is ApiOkResponse<MeterDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Compteur modifier avec succès"));

                    MeterStore?.UpdateMeter(new MeterDto(result.Result!.Id, Name, 0, DateTime.Now, SelectedUnitOfMeasurement?.Value.ToUnitMeasurementString(),
                        Frequency, SelectedFrequencyUnit.Value.ToString(), SelectedAssetId, SelectedAsset, null));
                    await CloseAsync();

                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de modidier du compteur"));
                }


            });
        }

        private async Task CreateMeter()
        {
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var meter = new MeterDtoForCreation(Name, 0, DateTime.Now, SelectedUnitOfMeasurement?.Value.ToUnitMeasurementString(), Frequency, SelectedAssetId, SelectedFrequencyUnit.Value.ToString());
                var response = await AppServices.ServiceManager.MeterService.CreateMeter(meter);
                if (response.Success && response is ApiOkResponse<MeterDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Compteur ajouté avec succès"));

                    MeterStore?.CreateMeter(new MeterDto(result.Result!.Id, Name, 0, DateTime.Now, SelectedUnitOfMeasurement?.Value.ToUnitMeasurementString(),
                        Frequency, SelectedFrequencyUnit.Value.ToString(), SelectedAssetId,SelectedAsset, null));
                    await CloseAsync();

                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de l'ajout du compteur"));
                }


            });
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();

        }

        public List<UnitOfMeasurementModel> GetUnitsOfMeasurement()
        {
            return Enum.GetValues(typeof(UnitMeasurement))
                       .Cast<UnitMeasurement>()
                       .Select(e => new UnitOfMeasurementModel
                       {
                           Value = e,
                           DisplayName = e.ToUnitMeasurementString()
                       })
                       .ToList();
        }

        public List<FrequencyUnitModel> GetFrequencyUnits()
        {
            return Enum.GetValues(typeof(FrequencyUnit))
                       .Cast<FrequencyUnit>()
                       .Select(e => new FrequencyUnitModel
                       {
                           Value = e,
                           DisplayName = e.ToString()
                       })
                       .ToList();
        }

        public override void Dispose()
        {
            SelectedAssetStore.AssetCreated -= OnSelectedEquipment;
            base.Dispose();

        }

    }
}
