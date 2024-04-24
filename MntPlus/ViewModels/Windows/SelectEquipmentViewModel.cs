using Entities.Responses.Equipment;
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
    public class SelectEquipmentViewModel : BaseViewModel
    {
        private SelectEquipmentItemViewModel _selectedViewModel;

        public SelectEquipmentItemViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                if (_selectedViewModel != value)
                {
                    if (_selectedViewModel != null)
                    {
                        _selectedViewModel.IsSelected = false;
                    }
                    _selectedViewModel = value;
                    if (_selectedViewModel != null)
                    {
                        _selectedViewModel.IsSelected = true;
                    }
                    OnPropertyChanged(nameof(SelectedViewModel));
                }
            }
        }

       
        public ObservableCollection<SelectEquipmentItemViewModel>? FilterEquipmentTreeViewItems { get; set; }


        private ObservableCollection<SelectEquipmentItemViewModel>? _equipmentTreeViewItems;
        public ObservableCollection<SelectEquipmentItemViewModel>? EquipmentTreeViewItems
        {
            get { return _equipmentTreeViewItems; }
            set
            {
                if (_equipmentTreeViewItems == value)
                    return;
                _equipmentTreeViewItems = value;
                FilterEquipmentTreeViewItems = new ObservableCollection<SelectEquipmentItemViewModel>(_equipmentTreeViewItems);
                OnPropertyChanged(nameof(EquipmentTreeViewItems));
            }
        }
        public ObservableCollection<EquipmentDto> EquipmentDtos { get; set; }

        public bool IsLoading { get; set; }

        public ICommand GetSelectedEquipmentCommand { get; set; }
        public SelectEquipmentViewModel()
        {
                _ = LoadDataAsync();
            if (EquipmentDtos is not null)
            {
                EquipmentTreeViewItems = CreateTreeViewItems(EquipmentDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentTreeViewItems);
            }
            GetSelectedEquipmentCommand = new RelayCommand(TheSelectedEquipment);
                
        }

        private void TheSelectedEquipment()
        {
            if(SelectedViewModel is not null)
            {
                SelectedViewModel.IsSelected = true;
            }
        }

        private async Task GetSelectedEquipment(SelectEquipmentItemViewModel selected)
        {
             SelectedViewModel = selected;
            //await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"Selected Equipment: {selected.Equipment.EquipmentName}"));
            await Task.Delay(1);
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            var Result = await AppServices.ServiceManager.EquipmentService.GetAllEquipmentsAsync(false);
            if (Result.Success && Result is ApiOkResponse<IEnumerable<EquipmentDto>> okResponse)
            {
                EquipmentDtos = new ObservableCollection<EquipmentDto>(okResponse.Result);
            }
            else
            if (Result is AssignorListNotFoundResponse response)
            {
                EquipmentDtos = new ObservableCollection<EquipmentDto>();
            }
            else if (Result is EquipmentGetListErrorResponse response1)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, response1.ErrorMessage));
            }
            await Task.Delay(1000);
            IsLoading = false;
        }

        private ObservableCollection<SelectEquipmentItemViewModel> CreateTreeViewItems(List<EquipmentDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<SelectEquipmentItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData.Where(e => e.EquipmentParent == null);

            foreach (var rootItem in rootItems)
            {
                var rootViewModel = new SelectEquipmentItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);

               // CalculateNumberOfChildren(rootViewModel, equipmentData);


                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }
        private void CreateTreeViewChildren(SelectEquipmentItemViewModel parentViewModel, List<EquipmentDto> equipmentData)
        {
            var children = equipmentData.Where(e => e.EquipmentParent == parentViewModel.Equipment.Id);

            foreach (var child in children)
            {
                var childViewModel = new SelectEquipmentItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel.Children.Add(childViewModel);
            }
        }
        void IterateEquipmentItemsAndChildren(ObservableCollection<SelectEquipmentItemViewModel> equipmentItems)
        {
            foreach (var equipmentItem in equipmentItems)
            {
                equipmentItem.SelectEquipmentFunc = GetSelectedEquipment;
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children);
                }
            }
        }
    }
}
