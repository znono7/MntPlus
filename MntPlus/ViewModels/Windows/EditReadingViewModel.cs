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
    public class EditReadingViewModel : BaseViewModel
    {
        public MeterReadingStore MeterStore { get; }
        public MeterReadingDto MeterReadingDto { get; }
        public string ReadingUnite { get; set; }
        public double ReadingValue { get; set; }
        public Guid MetreRedingId { get; }
        public Guid UserId { get; }
        public bool SaveIsRunning { get; set; }
        public ICommand SaveReadingCommand { get; set; }


        public EditReadingViewModel(MeterReadingStore meterStore,MeterReadingDto meterReadingDto,string unite)
        {
            MeterStore = meterStore;
            MeterReadingDto = meterReadingDto;
            ReadingUnite = unite;
            ReadingValue = meterReadingDto.Reading;
            MetreRedingId = meterReadingDto.Id;
            UserId = meterReadingDto.UserId ?? Guid.Empty;
            SaveReadingCommand = new RelayParameterizedCommand(async (p) => await SaveReadingAsync(p));
        }

        private async Task SaveReadingAsync(object? p)
        {
            var window = p as EditReadingWindow;
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var response = await AppServices.ServiceManager.MeterReadingService.UpdateMeterReading( MetreRedingId,
                    new MeterReadingDtoForCreation(MetreRedingId,ReadingValue,MeterReadingDto.Timestamp,UserId),true);
                if (response.Success && response is ApiOkResponse<MeterReadingDto> resultOk)
                {
                    MeterStore.UpdateMeterReading(resultOk.Result!);
                    window?.Close();
                }else if (response is ApiBadRequestResponse resultBad)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, resultBad.Message));
                }
               
            });
        }
    }
}
