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
        public ICommand DeleteCommand { get; set; }
        public ObservableCollection<PartInventoryItem>? PartItemViewModels { get; set; }
       public PartStore PartStore { get; set; }
        public AssetDto? AssetDto { get; }
        public EquipmentPartViewModel(AssetDto? assetDto) 
        {
            AssetDto = assetDto;

            //_ = LoadParts();
            AddEquipmentPartCommand = new RelayCommand(AddEquipmentPart);
            PartStore = new PartStore();
            PartStore.PartCreated += PartStore_PartAdded;
            LoadDataCommand = new RelayCommand(async () => await LoadParts());
            DeleteCommand = new RelayParameterizedCommand(async (p) => await DeletePart(p));
        }

        private async Task DeletePart(object? p)
        {
            if(p is not PartInventoryItem model)
            {
                return;
            }
            var response = await AppServices.ServiceManager.LinkPartService.DeleteLinkPart(model.Id!, AssetDto!.Id, true);
            if (response.Success) 
            {
                PartItemViewModels!.Remove(model);
            }
            else if (response is ApiBadRequestResponse errorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.Message));
            }

        }

        private async Task LoadParts()
        {
            try
            {
                var response = await AppServices.ServiceManager.AssetService.GetAllPartsAsync(AssetDto!.Id, false);
                if (response is ApiOkResponse<IEnumerable<PartDto>> okResponse)
                {
                    if (okResponse.Result != null)
                    {
                        PartDtos = new ObservableCollection<PartDto>(okResponse.Result);
                        PartItemViewModels = new ObservableCollection<PartInventoryItem>(okResponse.Result.Select(x => new PartInventoryItem(x)));

                    }
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
            if(dto == null)
            {return;}

            if (PartItemViewModels != null && PartItemViewModels.Any(x => x.Id == dto.Id))
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La pièce a déjà été ajoutée"));  
                return;
            };
            CreateLinkPart(dto).GetAwaiter().GetResult();
           
        }
        private async Task CreateLinkPart(PartDto? dto)
        {
            var response = await AppServices.ServiceManager.LinkPartService.CreateLinkPart(new LinkPartForCreationDto(dto!.Id, AssetDto!.Id));
            if (response is ApiOkResponse<LinkPartDto> okResponse)
            {
                PartItemViewModels ??= new ObservableCollection<PartInventoryItem>();
                PartItemViewModels.Add(new PartInventoryItem(dto));
            }
            else if (response is ApiBadRequestResponse errorResponse) 
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.Message));
            }
           
        }
        private void AddEquipmentPart()
        {
            SelectPartWindow selectPartWindow = new() { DataContext = new SelectPartViewModel(PartStore) };
            selectPartWindow.ShowDialog();
        }
    }
}
