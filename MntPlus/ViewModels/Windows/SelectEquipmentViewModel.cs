using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

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
        public ObservableCollection<AssetDto> EquipmentDtos { get; set; }

        public bool IsLoading { get; set; }

        public ICommand GetSelectedEquipmentCommand { get; set; }
        public WorkOrderStore? WorkOrderStore { get; set; }
        public AssetStore? AssetStore { get; set; }

        protected string? mLastSearchText;
        protected string? mSearchText;
        public ICommand SearchCommand { get; set; }

        public string? SearchText
        {
            get => mSearchText;
            set
            {
                // Check value is different
                if (mSearchText == value)
                    return;

                // Update value
                mSearchText = value;

                // If the search text is empty...
                if (string.IsNullOrEmpty(SearchText))
                    // Search to restore messages
                    Search();
            }
        }
        public SelectEquipmentViewModel(AssetStore? assetStore)
        {
            AssetStore = assetStore;
            //GenerateData();
            _ = LoadDataAsync();
            if (EquipmentDtos is not null) 
            {
                EquipmentTreeViewItems = CreateTreeViewItems(EquipmentDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentTreeViewItems);
            }
            GetSelectedEquipmentCommand = new RelayParameterizedCommand((p) => TheSelectedEquipment(p));
            SearchCommand = new RelayCommand(Search);

        }

        private void GenerateData()
        {
            EquipmentDtos = new ObservableCollection<AssetDto>
            {
                new AssetDto(Guid.Parse("CF0517C7-D792-4CAF-969F-D62226BCE1DC"),null,null,"Asset 1","Description 1","en service",null,null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),
                new AssetDto(Guid.Parse("5DD77287-1606-4336-8D2C-BAAE9F49534F"),null,null,"Asset 2","Description 2","en service",null,null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),
                new AssetDto(Guid.Parse("C3D3D3D3-3D3D-3D3D-3D3D-3D3D3D3D3D3D"),null,null,"Asset 3","Description 3","en service","Informatiques et de Bureau",null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),


                new AssetDto(Guid.Parse("BB96AA0A-D6C4-468B-BC0F-FBB404B79469"),Guid.Parse("CF0517C7-D792-4CAF-969F-D62226BCE1DC"),
                             new AssetDto(Guid.Parse("CF0517C7-D792-4CAF-969F-D62226BCE1DC"),null,null,"Asset 1","Description 1","en service",null,null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),
                "Asset 4","Description 4","en service",null,null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),
                new AssetDto(Guid.Parse("59C1F0CC-46C6-4104-B0A9-31E2A6DA3A1C"),Guid.Parse("CF0517C7-D792-4CAF-969F-D62226BCE1DC"),
                             new AssetDto(Guid.Parse("CF0517C7-D792-4CAF-969F-D62226BCE1DC"),null,null,"Asset 1","Description 1","en service","Informatiques et de Bureau",null,null,"12500365","modelsdd",null,12500,null,null,null,null,null)
                ,"Asset 5","Description 5","en service","Informatiques et de Bureau",null,null,"12500365","modelsdd",null,12500,null,null,null,null,null),
            };
        }


        private void TheSelectedEquipment(object? p)
        {
            if (SelectedViewModel is null || !(p is Window wind) || SelectedViewModel == null)
                return;

            SelectedViewModel.IsSelected = true;

            if (p is not Window)
                return;

            AssetStore?.CreateAsset(SelectedViewModel.Equipment);
            //var window = new StartManageWorkWindow { DataContext = new StartManageWorkWindowViewModel(SelectedViewModel.Equipment,WorkOrderStore) }; 
            
            //window.ShowDialog();
            

            wind.Close();
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
            var Result = await AppServices.ServiceManager.AssetService.GetAllAssetsAsync(false);
            if (Result.Success && Result is ApiOkResponse<IEnumerable<AssetDto>> okResponse)
            {
                EquipmentDtos = new ObservableCollection<AssetDto>(okResponse.Result);
            }
            else
            if (Result is ApiNotFoundResponse response)
            {
                EquipmentDtos = new ObservableCollection<AssetDto>();
            }
            else if (Result is ApiBadRequestResponse response1)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, response1.Message));
            }
            await Task.Delay(1000);
            IsLoading = false;
        }

        private ObservableCollection<SelectEquipmentItemViewModel> CreateTreeViewItems(List<AssetDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<SelectEquipmentItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData.Where(e => e.AssetParent == null);

            foreach (var rootItem in rootItems)
            {
                var rootViewModel = new SelectEquipmentItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);

               // CalculateNumberOfChildren(rootViewModel, equipmentData);


                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }
        private void CreateTreeViewChildren(SelectEquipmentItemViewModel parentViewModel, List<AssetDto> equipmentData)
        {
            var children = equipmentData.Where(e => e.AssetParent == parentViewModel.Equipment.Id);

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

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            SearchEquipmentHelper searchHelper = new();


            if (string.IsNullOrEmpty(SearchText) || EquipmentTreeViewItems is null || EquipmentTreeViewItems.Count <= 0)
            {
                // Make filtered list the same
                FilterEquipmentTreeViewItems = new ObservableCollection<SelectEquipmentItemViewModel>(EquipmentTreeViewItems ?? Enumerable.Empty<SelectEquipmentItemViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            SearchEquipmentHelper search = new();
            CreateEquipmentListItems listItems = new();
            FilterEquipmentTreeViewItems = new ObservableCollection<SelectEquipmentItemViewModel>();
            var EquipmentSearched = search.SearchItems(EquipmentDtos.ToList(), SearchText);
            FilterEquipmentTreeViewItems = listItems.CreateListItemsForSelection(EquipmentSearched.ToList());

            // Set last search text
            mLastSearchText = SearchText;

        }
    }
}
