using Accessibility;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF.ViewModels.Windows
{
    public class SelectPartViewModel : BaseViewModel
    {
        public PartStore PartStore { get; }

        public PartInventoryItem? SelectedPart { get; set; }

        public ObservableCollection<PartInventoryItem>? PartItemViewModels { get; set; }

        public ICommand SelectPartCommand { get; set; }
        public ICommand GetSelectedPartCommand { get; set; }


        public SelectPartViewModel(PartStore partStore)
        {
            _ = GetAllParts();
            PartStore = partStore;
            SelectPartCommand = new RelayParameterizedCommand((p) => SelectPartFunc(p));
            GetSelectedPartCommand = new RelayParameterizedCommand((p) => GetSelectPartFunc(p));
        } 
        private async Task GetAllParts()
        {
            var response = await AppServices.ServiceManager.PartService.GetAllPartsAsync(false);
            if (response is ApiOkResponse<IEnumerable<PartDto>> result && result.Success)
            {
                PartItemViewModels = new ObservableCollection<PartInventoryItem>(result.Result!.Select(x => new PartInventoryItem(x)));
            }
            else if (response is ApiBadRequestResponse badResp)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResp.Message));
            }
        }
        private void GetSelectPartFunc(object? p)
        {
            if(p is SelectPartWindow partWind)
            {
                if(SelectedPart != null)
                {
                    PartStore.CreatePart(SelectedPart.Part);
                    partWind.Close();
                    
                }
                else
                {
                    IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une pièce pour continuer"));
                }
            }
        }

        private void SelectPartFunc(object? p)
        {
            if(p is PartInventoryItem part)
            {
                SelectedPart = part;
            }
        }
    }
}
