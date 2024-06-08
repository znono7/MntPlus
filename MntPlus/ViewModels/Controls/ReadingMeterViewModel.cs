using Azure;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ReadingMeterViewModel : BaseViewModel
    {
        public ObservableCollection<MeterReadingDto>? MeterReadings { get; set; }
        public bool SaveIsRunning { get; set; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }

        public ICommand EditMeterCommand { get; set; }
        public Func<MeterDto,Task>? EditAction { get; set; }

        public ICommand DeleteMeterCommand { get; set; }
        public string? NextReading { get; set; }
        public MeterDto? MeterDto { get; }
        public MeterStore? MeterStore { get; }

        public string? LastReading => $"Dernière lecture : {MeterDto?.CurrentReading.ToString("N2")}";
        public string? Frequency => $"Fréquence de lecture du compteur: Chaque {MeterDto?.Frequency} {MeterDto?.FrequencyUnit}";
        public ICommand SaveReadingCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditReadingCommand { get; set; }
        public double ReadingValue { get; set; }

        public MeterReadingStore MeterReadingStore { get; set; }

        public string ReadingUnite { get; set; } 
        public ReadingMeterViewModel(MeterDto? meterDto , DateTime? nextReading , MeterStore? meterStore)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            MeterDto = meterDto;
            MeterStore = meterStore;
            NextReading = $"Lecture suivante: {nextReading}";
            ReadingUnite = MeterDto?.Unit ?? string.Empty;
            SaveReadingCommand = new RelayCommand(async () => await SaveReadingAsync());
            DeleteCommand = new RelayParameterizedCommand(async (p) => await DeleteReadingAsync(p));
            EditReadingCommand = new RelayParameterizedCommand(async (p) => await EditReadingAsync(p));
            DeleteMeterCommand = new RelayCommand(async () => await DeleteMeterAsync());
            _ = GetReadings();
            EditMeterCommand = new RelayCommand(async () => await EditMeterAsync());
        }

        private async Task EditReadingAsync(object? p)
        {
            if (p is MeterReadingDto meterReadingDto)
            {
                MeterReadingStore = new MeterReadingStore();
                MeterReadingStore.MeterReadingUpdated += UpdateReadingValue;
                var window = new EditReadingWindow();
                var viewModel = new EditReadingViewModel(MeterReadingStore, meterReadingDto, MeterDto?.Unit!);
                window.DataContext = viewModel;
                window.ShowDialog();
            }
            await Task.Delay(1);
        }

        private void UpdateReadingValue(MeterReadingDto meterReadingDto)
        {
            var findReading = MeterReadings?.FirstOrDefault(c => c.Id == meterReadingDto.Id);
            if (findReading != null)
            {
                //find index of the reading
                var index = MeterReadings!.IndexOf(findReading);
                MeterReadings[index] = meterReadingDto;
                var lastAddedItem = MeterReadings?.OrderByDescending(c => c.Timestamp).FirstOrDefault();
                if (lastAddedItem != null && lastAddedItem == meterReadingDto)
                {
                    UpdateMeter(new MeterDto(MeterDto!.Id,MeterDto.Name,meterReadingDto.Reading,MeterDto.LastUpdated,MeterDto.Unit,MeterDto.Frequency,
                        MeterDto.FrequencyUnit,MeterDto.AssetId,MeterDto.AssetName,null)).GetAwaiter().GetResult();
                }
               
            }
        }
        private async Task UpdateMeter(MeterDto meterDto)
        {
            var result = await AppServices.ServiceManager.MeterService.UpdateMeter(meterDto.Id, 
                new MeterDtoForCreation(meterDto.Name, meterDto.CurrentReading, meterDto.LastUpdated, meterDto.Unit, meterDto.Frequency, meterDto.AssetId, meterDto.FrequencyUnit), true);
            if (result.Success)
            {
                MeterStore!.UpdateMeter(meterDto);
            }
        }
        private async Task EditMeterAsync()
        {
            if (EditAction != null)
                await EditAction(MeterDto!);
        }

        private async Task DeleteMeterAsync()
        {
            var Dialog = new ConfirmationWindow("Supprimé Le compteur");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
                return;
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var result = await AppServices.ServiceManager.MeterService.DeleteMeter(MeterDto!.Id,false);
                if (result.Success)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Le compteur a été supprimé avec succès"));
                    MeterStore!.DeleteMeter(MeterDto);
                    await CloseAsync();
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la suppression du compteur"));
                }
            });
        }

        private async Task DeleteReadingAsync(object? p)
        {
            var Dialog = new ConfirmationWindow("Supprimé La lecture");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
                return;

            if (p is MeterReadingDto meterReadingDto)
            {
                var lastAddedItem = MeterReadings?.OrderByDescending(c => c.Timestamp).FirstOrDefault();
                if (lastAddedItem != null && lastAddedItem == meterReadingDto)
                {
                    await RunCommandAsync (() => SaveIsRunning, async () =>
                    {
                        var result = await AppServices.ServiceManager.MeterReadingService.DeleteUpdateMeterReading(meterReadingDto.Id, true);
                        if (result.Success)
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La lecture a été supprimée avec succès"));
                            MeterReadings!.Remove(meterReadingDto);
                            MeterStore!.UpdateMeter(new MeterDto(MeterDto?.Id ?? Guid.Empty, MeterDto?.Name, 0, meterReadingDto.Timestamp, MeterDto!.Unit, MeterDto!.Frequency, MeterDto!.FrequencyUnit, MeterDto!.AssetId,MeterDto.AssetName,null));
                        }
                        else if(result is ApiBadRequestResponse responseBadRequest)
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, responseBadRequest.Message));
                        }
                        else
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la suppression de la lecture"));
                        }
                      
                    });

                }
                else
                {
                    await RunCommandAsync(() => SaveIsRunning, async () =>
                    {
                        var result = await AppServices.ServiceManager.MeterReadingService.DeleteMeterReading(meterReadingDto.Id, true);
                        if (result.Success)
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La lecture a été supprimée avec succès"));
                            MeterReadings!.Remove(meterReadingDto);

                        }
                        else if (result is ApiBadRequestResponse responseBadRequest)
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, responseBadRequest.Message));
                        }
                        else
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la suppression de la lecture"));
                        }
                    });

                }
            }
               


               
        }

        private async Task GetReadings()
        {
            var result = await AppServices.ServiceManager.MeterReadingService.GetAllMeterReadingsAsync(MeterDto!.Id,false);
            if (result.Success && result is ApiOkResponse<IEnumerable<MeterReadingDto>> responseOk)
            { 
                MeterReadings = new ObservableCollection<MeterReadingDto>(responseOk.Result!);
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la récupération des lectures"));
            }
        }
        private async Task SaveReadingAsync()
        {
            if (ReadingValue <= 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La valeur de la lecture doit être supérieure à 0"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var result = await AppServices.ServiceManager.MeterReadingService.CreateMeterReading(new 
                    MeterReadingDtoForCreation(
                    MeterDto!.Id,
                    ReadingValue,
                    DateTime.Now,
                    Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E")
                    ));
                    
                if (result.Success && result is ApiOkResponse<MeterReadingDto> responsOk)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La lecture a été enregistrée avec succès"));
                    var resp = await AppServices.ServiceManager.MeterService.UpdateMeter(MeterDto!.Id, 
                        new MeterDtoForCreation(MeterDto?.Name,ReadingValue,responsOk.Result!.Timestamp,MeterDto!.Unit,MeterDto!.Frequency,MeterDto!.AssetId,MeterDto!.FrequencyUnit), 
                        true);
                    MeterStore!.UpdateMeter(new MeterDto(MeterDto?.Id ?? Guid.Empty, MeterDto?.Name, ReadingValue, responsOk.Result!.Timestamp, MeterDto!.Unit, MeterDto!.Frequency, MeterDto!.FrequencyUnit, MeterDto!.AssetId,MeterDto.AssetName,null));
                    MeterReadings!.Add(responsOk.Result!);
                    await CloseAsync();
                }
                else if (result is ApiBadRequestResponse responseBadRequest)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, responseBadRequest.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de l'enregistrement de la lecture"));
                }
            });
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();
        }
    }
}
