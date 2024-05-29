using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentPageViewModel : BaseViewModel
    {
        #region protected
        protected string? mLastSearchText;
        protected string? mSearchText;

        #endregion
        #region Public Properties

        public ObservableCollection<TagControlViewModel>? FilterTags { get; set; }
        public ObservableCollection<FilterCriteria> AppliedFilters { get; set; } 

        private ObservableCollection<EquipmentItemViewModel>? equipmentItemViewModels { get; set; }
        public ObservableCollection<EquipmentItemViewModel>? EquipmentItemViewModels
        {
            get { return equipmentItemViewModels; }
            set
            {
                if (equipmentItemViewModels == value)
                    return;
                equipmentItemViewModels = value;
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>(equipmentItemViewModels!);
                OnPropertyChanged(nameof(EquipmentItemViewModels));
            }
        }
        public ObservableCollection<EquipmentItemViewModel>? FilterEquipmentItemViewModels { get; set; }

        public ObservableCollection<AssetDto>? AssetDtos { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMenu2Open { get; set; }
        public bool IsHeaderVisible { get; set; } //=> (IsList && !IsEmpty) || (IsHierarchy && !IsEmpty);
        public bool IsList { get; set; }
        public bool IsHierarchy { get; set; }
        public bool IsLoading { get; set; } 
        public bool IsEmpty { get; set; } //=> EquipmentListItems is null || EquipmentListItems.Count == 0 ;
        private AssetStore _equipmentStore { get; set; }

        public bool IsFilterOpen { get; set; }
        public ICommand OpenFilterCommand { get; set; }

       

        #endregion

        #region Commands  
        public ICommand MenuCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }   
        public ICommand ImpExpCommand { get; set; }
        public ICommand ToListCommand { get; set; }
        public ICommand TohierarchyCommand { get; set; }
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

        public FilterAssetControlViewModel FilterAssetControl { get; set; }
        #endregion

        public EquipmentPageViewModel()
        {
            SearchCommand = new RelayCommand(Search);
            MenuCommand = new RelayCommand(() => IsMenuOpen = !IsMenuOpen);
            ImpExpCommand = new RelayCommand(() => IsMenu2Open = !IsMenu2Open);
            AddEquipmentCommand = new RelayCommand(AddEquipment);
            _equipmentStore = new AssetStore();
            _equipmentStore.AssetCreated += OnEquipmentCreated;
            _equipmentStore.AssetUpdated += OnEquipmentUpdated;
          
           
            _ = LoadDataAsync();
           // GenerateData();
           
            if (AssetDtos is not null && AssetDtos.Count > 0)
            {
                CreateEquipmentTree equipmentTree = new();
                EquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                EquipmentItemViewModels = equipmentTree.CreateTreeViewItems(AssetDtos.ToList());
            }
            FilterAssetControl = new FilterAssetControlViewModel();
            FilterAssetControl.FilterByCategoryFonction = FilterByCategory;
            OpenFilterCommand = new RelayCommand(() => IsFilterOpen = !IsFilterOpen);
            FilterTags = new ObservableCollection<TagControlViewModel>();
            AppliedFilters = new ObservableCollection<FilterCriteria>();


        }

        private async Task FilterByCategory(EquipmentCategory category)
        {
            if(EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Category:", category.Name,EquipmentFilterType.Category);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.Category == category.Name).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.Category, category.Name);
                AppliedFilters.Add(filterCriteria);
               
            }else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
        }

        private async Task RemoveTag(EquipmentFilterType arg)
        {
            if(FilterTags is not null && FilterTags.Count > 0)
            {
                var tag = FilterTags.FirstOrDefault(t => t.EquipmentFilterType == arg);
                var filt = AppliedFilters.FirstOrDefault(f => f.EquipmentFilterType == arg);
                if (tag is not null && filt is not null)
                {
                    FilterTags.Remove(tag);
                    RemoveFilter(filt);
                    //if(FilterTags.Count == 0)
                    //{
                    //    FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>(EquipmentItemViewModels ?? Enumerable.Empty<EquipmentItemViewModel>());
                    //}
                }
            }
            await Task.Delay(1);
        }

        private void GenerateData()
        {
            AssetDtos = new ObservableCollection<AssetDto>
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
        private void OnEquipmentUpdated(AssetDto? dto)
        {
            //EquipmentItemViewModels
            EquipmentItemViewModel? EquipmentItem = EquipmentItemViewModels?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
            if (EquipmentItem is not null && dto is not null)
            {
                EquipmentItem.Asset = dto;
              
            }
           
            IsHeaderVisible = true;
            IsEmpty = false;

        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            SearchEquipmentHelper searchHelper = new();

            
                if (string.IsNullOrEmpty(SearchText) || EquipmentItemViewModels is null || EquipmentItemViewModels.Count <= 0)
                {
                    // Make filtered list the same
                    FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>(EquipmentItemViewModels ?? Enumerable.Empty<EquipmentItemViewModel>());

                    // Set last search text
                    mLastSearchText = SearchText;

                    return;
                }
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>( searchHelper.SearchItems(EquipmentItemViewModels, SearchText) );
                // Set last search text
                mLastSearchText = SearchText;
           
        }

        private void OnEquipmentCreated(AssetDto? newEquipment)
        {
            var newItemViewModel = new EquipmentItemViewModel(newEquipment);
            FindEquipmentParentViewModel findEquipmentParentViewModel = new(EquipmentItemViewModels);


            var parentViewModel = findEquipmentParentViewModel.FindParentViewModel(newEquipment?.AssetParent);
           

            if (parentViewModel is not null)
            {
                parentViewModel?.Children?.Add(newItemViewModel);
            }
            else
            {
                EquipmentItemViewModels ??= new ObservableCollection<EquipmentItemViewModel>();
                EquipmentItemViewModels?.Add(newItemViewModel);
                FilterEquipmentItemViewModels ??= new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels?.Add(newItemViewModel);
            }

          

        }

       
      
        public async Task LoadDataAsync()
        {
            await RunCommandAsync( () => IsLoading , async () =>
            {
                var Result = await AppServices.ServiceManager.AssetService.GetAllAssetsAsync(false); 

                if ( Result.Success && Result is ApiOkResponse<IEnumerable<AssetDto>> okResponse)
                {
                    AssetDtos = new ObservableCollection<AssetDto>(okResponse.Result!);

                    
                   
                }
                
                if (Result is ApiNotFoundResponse response)
                {
                    IsEmpty = true;
                    AssetDtos = new ObservableCollection<AssetDto>();

                   
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));

                }
                 if (!Result.Success && Result is ApiBadRequestResponse response1)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, response1.Message));

                  
                }
            });
           
        }
      
        private void AddEquipment()
        {
                
            NewEquipmentWindow window = new(_equipmentStore);          
            window.ShowDialog();
        }


        private async Task RemoveItem(EquipmentItemViewModel cmodel)
        {
            if(cmodel.ChildrenCount > 0 && cmodel.Children?.Count > 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Vous ne pouvez pas supprimer cet équipement car il contient des enfants"));
                return;
            }
            var Dialog = new ConfirmationWindow(cmodel.Asset?.Name);
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
                var response = await AppServices.ServiceManager.AssetService.DeleteAsset(cmodel.Asset.Id,false);
                if (response is ApiOkResponse<AssetDto> && response.Success)
                {
                    RemoveEquipmentItemFromTreeView removeEquipmentItem = new(EquipmentItemViewModels);
                    // Remove the item from the tree view
                    removeEquipmentItem.RemoveItemFromTreeView(cmodel);

                    // Remove the item from the list view
                    EquipmentItemViewModels?.Remove(cmodel);
                    
                    if (EquipmentItemViewModels?.Count == 0)
                    {
                        IsHeaderVisible = false;
                        IsEmpty = true;
                    }
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipement Supprimé avec Succès"));
                }else if(response is ApiBadRequestResponse errorResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'équipement"));
                }
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Suppression Annulée"));
            }
               
        }
       

        void IterateEquipmentItemsAndChildren(ObservableCollection<EquipmentItemViewModel> equipmentItems)
        {
            foreach (var equipmentItem in equipmentItems)
            {
                
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children);
                }
            }
        }

       

       
        public override void Dispose()
        {
            _equipmentStore.AssetCreated -= OnEquipmentCreated;
            base.Dispose();

        }



        #region FilterMethods
        public void RemoveFilter(FilterCriteria filterToRemove)
        {
            AppliedFilters.Remove(filterToRemove);
            
            if(AppliedFilters.Count == 0)
            {
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>(EquipmentItemViewModels ?? Enumerable.Empty<EquipmentItemViewModel>());
            }
            else
            {
                ApplyFilters();
            }
            
        }

        public void ApplyFilters()
        {
            if(AssetDtos == null || AssetDtos.Count == 0)
            {
                return;
            }
            // Get the original data source (AssetDtos in your case)
            var filteredItems = AssetDtos.ToList(); // Make a copy to preserve the original data

            // Apply each filter in the list of applied filters
            foreach (var filter in AppliedFilters)
            {
                // Apply the filter to the filteredItems collection
                filteredItems = ApplyFilter(filteredItems, filter);
            }

            CreateEquipmentListItems listItems = new();

            // Update the FilterEquipmentItemViewModels collection with the filtered items
            FilterEquipmentItemViewModels = listItems.CreateListItems(filteredItems);
        }

        private List<AssetDto> ApplyFilter(List<AssetDto> items, FilterCriteria filter)
        {
            // Apply the filter based on the filter criteria
            switch (filter.EquipmentFilterType)
            {
                case EquipmentFilterType.Category:
                    return items.Where(e => e.Category == filter.FilterValue).ToList();
                // Add cases for other filter types (e.g., by name, by date, etc.)
                default:
                    return items; // If the filter type is unknown or not supported, return the original collection
            }
        }
        #endregion









    }
}
