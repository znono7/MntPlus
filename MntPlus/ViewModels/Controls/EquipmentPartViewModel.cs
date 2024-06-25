using Entities;
using MntPlus.WPF.ViewModels.Windows;
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
    public class EquipmentPartViewModel : BaseViewModel
    {
        public ObservableCollection<PartDto>? PartDtos { get; set; }
        public ICommand AddEquipmentPartCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ObservableCollection<PartInventoryItem>? PartItemViewModels { get; set; }
       public PartStore PartStore { get; set; }
        public AssetDto? AssetDto { get; }

        public EquipmentPartViewModel(AssetDto? assetDto) 
        {
            AddEquipmentPartCommand = new RelayCommand(AddEquipmentPart);
            PartStore = new PartStore();
            PartStore.PartCreated += PartStore_PartAdded;
            AssetDto = assetDto;
            LoadDataCommand = new RelayCommand(async () => await LoadParts());
        }

        private async Task LoadParts()
        {
            try
            {
                var response = await AppServices.ServiceManager.AssetService.GetAllPartsAsync(AssetDto!.Id, false);
                if (response is ApiOkResponse<IEnumerable<PartDto>> okResponse)
                {
                    if (okResponse.Result != null)
                        PartDtos = new ObservableCollection<PartDto>(okResponse.Result);
                }
                else if (response is ApiErrorResponse errorResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ex.Message));
            }
        }
        private void PartStore_PartAdded(PartDto? dto)
        {

            Task.Run(async () =>
            {
                try
                {
                    await CreateLinkPart(dto);
                }
                catch (Exception ax)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ax.Message));
                }
            });
            
        }
        private async Task CreateLinkPart(PartDto? dto)
        {
            var response = await AppServices.ServiceManager.LinkPartService.CreateLinkPart(new LinkPartForCreationDto(dto!.Id, AssetDto!.Id));
            if (response is ApiOkResponse<LinkPartDto> okResponse)
            {
                PartItemViewModels ??= new ObservableCollection<PartInventoryItem>();
                PartItemViewModels.Add(new PartInventoryItem(dto));
            }
            else if (response is ApiErrorResponse errorResponse) 
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.ErrorMessage));
            }
           
        }
        private void AddEquipmentPart()
        {
            SelectPartWindow selectPartWindow = new() { DataContext = new SelectPartViewModel(PartStore) };
            selectPartWindow.ShowDialog();
        }
    }
}
