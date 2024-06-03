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
    public class PartsInventoryViewModel : BaseViewModel
    {
        public ObservableCollection<PartInventoryItem>? FilterPartItemViewModels { get; set; }

        private ObservableCollection<PartInventoryItem>? _partItemViewModels { get; set; }
        public ObservableCollection<PartInventoryItem>? PartItemViewModels 
        { 
            get => _partItemViewModels;   
            set
            {
                _partItemViewModels = value;
                FilterPartItemViewModels = new ObservableCollection<PartInventoryItem>(_partItemViewModels!);

            } 
        }

        public AddPartControlViewModel addPartControl { get; set; }

        public bool AddPartPopupIsOpen { get; set; }
        public bool DimmableOverlayVisible { get; set; }

        public ICommand OpenControlCommand { get; set; }
        public ICommand OpenActionPopupOpenCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public bool IsActionPopupOpen { get;  set; }

        public PartStore PartStore { get; set; }

        public PartsInventoryViewModel()
        {
            _ = GetAllParts();
            PartStore =   new PartStore();
            PartStore.PartCreated += PartStore_PartAdded;
            addPartControl = new AddPartControlViewModel(PartStore);
            addPartControl.CloseAction += CloseControl;
            OpenControlCommand = new RelayCommand(() => { AddPartPopupIsOpen = true; DimmableOverlayVisible = true; });
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            RemoveCommand = new RelayCommand(async () => await RemoveItem());

        }
        public ObservableCollection<PartDto>? PartDtoItems { get; set; }
        private async Task GetAllParts()
        {
            var response = await AppServices.ServiceManager.PartService.GetAllPartsAsync(false);
            if (response is ApiOkResponse<IEnumerable<PartDto>> result && result.Success)
            {
                PartItemViewModels = new ObservableCollection<PartInventoryItem>(result.Result!.Select(x => new PartInventoryItem(x)));
            }else if (response is ApiBadRequestResponse badResp)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResp.Message));
            }
        }
        private void PartStore_PartAdded(PartDto? dto)
        {
            FilterPartItemViewModels ??= new ObservableCollection<PartInventoryItem>();
            FilterPartItemViewModels.Add(new PartInventoryItem(dto!));
            PartItemViewModels ??= new ObservableCollection<PartInventoryItem>();
            PartItemViewModels.Add(new PartInventoryItem(dto!));
        }

        private async Task RemoveItem()
        {
            List<PartInventoryItem> itemsToRemove = new();
            if (FilterPartItemViewModels is not null && FilterPartItemViewModels.Count > 0)
            {
                foreach (var item in FilterPartItemViewModels)
                {
                    if (item.IsChecked)
                    {
                        itemsToRemove.Add(item);

                    }
                }
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun piéce sélectionné"));
                return;
            }
            if (!itemsToRemove.Any())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun piéce sélectionné"));
                return;
            }
            var Dialog = new ConfirmationWindow("Supprimé Piéce");
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
                foreach (var item in itemsToRemove)
                {
                    await RemoveEquipment(item);
                }
            }

        }
        private async Task RemoveEquipment(PartInventoryItem part)
        {
            var response = await AppServices.ServiceManager.PartService.DeletePart(part.Id, false);
            if (response is ApiOkResponse<PartDto> && response.Success)
            {
               
                // Remove the item from the list view
                PartItemViewModels?.Remove(part);
                FilterPartItemViewModels?.Remove(part);


            }

        }


        private async Task CloseControl()
        {
            AddPartPopupIsOpen = false; DimmableOverlayVisible = false;
            await Task.Delay(1);
        }
    }
}
 