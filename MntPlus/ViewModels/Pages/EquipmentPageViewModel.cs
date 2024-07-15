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
        public bool IsActionPopupOpen { get; set; }
        private AssetStore _equipmentStore { get; set; }

        public bool IsFilterOpen { get; set; }
        public ICommand OpenFilterCommand { get; set; }
        public ICommand OpenActionPopupOpenCommand { get; set; }
        public ICommand RemoveCommand { get; set; }


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
            _equipmentStore.AssetDeleted += OnEquipmentDeleted;

            _ = LoadDataAsync();
           
            if (AssetDtos is not null && AssetDtos.Count > 0)
            {
                CreateEquipmentTree equipmentTree = new();
                EquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                EquipmentItemViewModels = equipmentTree.CreateTreeViewItems(AssetDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentItemViewModels);
            }
            FilterAssetControl = new FilterAssetControlViewModel();
            FilterAssetControl.FilterByCategoryFonction = FilterByCategory;
            FilterAssetControl.FilterByStatutFonction = FilterByStatut;
            FilterAssetControl.FilterBySerialNumberFonction = FilterBySerialNumber;
            FilterAssetControl.FilterByModelFonction = FilterByModel;
            FilterAssetControl.FilterByMakeFonction = FilterByMake;
            FilterAssetControl.FilterByDateServiceFonction = FilterByDateService;
            FilterAssetControl.FilterByDateCreatedFonction = FilterByDateCreated;
            FilterAssetControl.ResetFunction = ApplyResetFunction;


            OpenFilterCommand = new RelayCommand(() => IsFilterOpen = !IsFilterOpen);
            FilterTags = new ObservableCollection<TagControlViewModel>();
            AppliedFilters = new ObservableCollection<FilterCriteria>();

            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            RemoveCommand = new RelayCommand(async () => await RemoveItem());


        }

        private void OnEquipmentDeleted(AssetDto? dto)
        {
            if (dto != null)
            {
                var equip = EquipmentItemViewModels?.FirstOrDefault(x => x.Asset?.Id == dto.Id);
                if(equip != null)
                {
                    OnRemoveAsset(equip);
                    EquipmentItemViewModels?.Remove(equip);
                    FilterEquipmentItemViewModels?.Remove(equip);
                }

            }
        }

        public List<EquipmentItemViewModel> GetSelectedChildren(EquipmentItemViewModel parent)
        {
            List<EquipmentItemViewModel> selectedItems = new List<EquipmentItemViewModel>();

            if (parent.IsChecked)
            {
                selectedItems.Add(parent);
            }

            if (parent.Children != null)
            {
                foreach (var child in parent.Children)
                {
                    selectedItems.AddRange(GetSelectedChildren(child));
                }
            }

            return selectedItems;
        }

        private async Task RemoveItem()
        {
           
            List<EquipmentItemViewModel> itemsToRemove = new();
            if (FilterEquipmentItemViewModels is not null && FilterEquipmentItemViewModels.Count > 0)
            {
                foreach (var item in FilterEquipmentItemViewModels)
                {
                    itemsToRemove.AddRange(GetSelectedChildren(item));
                }
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun équipement sélectionné"));
                return;
            }
            if (!itemsToRemove.Any())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun équipement sélectionné"));
                return;
            }
            var Dialog = new ConfirmationWindow("Supprimé Equipement");
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
               if(itemsToRemove.Count > 1)
                {
                    await RemoveEquipItems(itemsToRemove);
                }
                else
                {
                    await RemoveEquipment(itemsToRemove[0]);
                }
            }           

        }

        private async Task RemoveEquipItems(List<EquipmentItemViewModel> itemsToRemove)
        {
            List<Guid> assetIds = new();
            foreach (var item in itemsToRemove)
            {
                assetIds.Add(item.Asset!.Id);
            }
            var response = await AppServices.ServiceManager.AssetService.DeleteAssets(assetIds, true);
            if (response is ApiOkResponse<AssetDto> && response.Success)
            {
                foreach (var item in itemsToRemove)
                {
                    RemoveEquipmentItemFromTreeView removeEquipmentItem = new(EquipmentItemViewModels);
                    // Remove the item from the tree view
                    removeEquipmentItem.RemoveItemFromTreeView(item);

                    // Remove the item from the list view
                    EquipmentItemViewModels?.Remove(item);
                    FilterEquipmentItemViewModels?.Remove(item);
                    AssetDtos?.Remove(item.Asset!);
                }
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipements Supprimés avec Succès"));
            }
            else if (response is ApiBadRequestResponse errorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur : Impossible de supprimer l'enregistrement car il est référencé par d'autres enregistrements."));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'équipement"));
            }
        }

        private async Task RemoveEquipment(EquipmentItemViewModel equipment)
        {
            var response = await AppServices.ServiceManager.AssetService.DeleteAsset(equipment.Asset!.Id, true);
            if (response is ApiOkResponse<AssetDto> && response.Success)
            {
                OnRemoveAsset(equipment);

                // Remove the item from the list view
                EquipmentItemViewModels?.Remove(equipment);
                FilterEquipmentItemViewModels?.Remove(equipment);
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipement Supprimé avec Succès"));

            }
            else if(response is ApiBadRequestResponse errorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur : Impossible de supprimer l'enregistrement car il est référencé par d'autres enregistrements."));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'équipement"));
            }
           
        }

        private void OnRemoveAsset(EquipmentItemViewModel equipment)
        {
            RemoveEquipmentItemFromTreeView removeEquipmentItem = new(EquipmentItemViewModels);
            // Remove the item from the tree view
            removeEquipmentItem.RemoveItemFromTreeView(equipment);
        }

        private async Task ApplyResetFunction()
        {

            if (FilterTags is not null && FilterTags.Count > 0)
            {
                FilterTags.Clear();
                AppliedFilters.Clear();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>(EquipmentItemViewModels ?? Enumerable.Empty<EquipmentItemViewModel>());
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun filtre à réinitialiser"));
            }
            
        }

        private async Task FilterByDateCreated(DateTime? nullable1, DateTime? nullable2)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Date de Création:", $"{nullable1} - {nullable2}", EquipmentFilterType.DateCreated);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.CreatedDate >= nullable1 && e.CreatedDate <= nullable2).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.DateCreated, $"{nullable1} - {nullable2}");
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
            
        }

        private async Task FilterByDateService(DateTime? nullable1, DateTime? nullable2)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Date de Mise en Service:", $"{nullable1} - {nullable2}", EquipmentFilterType.DateService);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.AssetCommissionDate >= nullable1 && e.AssetCommissionDate <= nullable2).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.DateService, $"{nullable1} - {nullable2}");
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
            
        }

        private async Task FilterByMake(string? arg)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Marque:", arg, EquipmentFilterType.Make);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(
                    e => e.Make != null && e.Make.Contains(arg!, StringComparison.OrdinalIgnoreCase)).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.Make, arg);
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
        }

        private async Task FilterByModel(string? arg)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Modèle:", arg, EquipmentFilterType.Model);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(
                    e => e.Model!=null && e.Model.Contains(arg!,StringComparison.OrdinalIgnoreCase)).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.Model, arg);
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
            
        }

        private async Task FilterBySerialNumber(string? arg)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Numéro de série:", arg, EquipmentFilterType.SerialNumber);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.SerialNumber != null && arg != null &&
                e.SerialNumber.Contains(arg)).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.SerialNumber, arg);
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
            
        }

        private async Task FilterByStatut(AssetStatus status)
        {
            if (EquipmentItemViewModels is not null && EquipmentItemViewModels.Count > 0 && AssetDtos is not null)
            {
                TagControlViewModel tagControlViewModel = new("Category:", status.Name, EquipmentFilterType.Statut);
                tagControlViewModel.CancelTagFonction = RemoveTag;
                FilterTags ??= [];
                FilterTags.Add(tagControlViewModel);
                CreateEquipmentListItems listItems = new();
                FilterEquipmentItemViewModels = new ObservableCollection<EquipmentItemViewModel>();
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.Status != null && status.Name != null &&
                e.Status.Contains(status.Name, StringComparison.OrdinalIgnoreCase)).ToList());
                FilterCriteria filterCriteria = new(EquipmentFilterType.Statut, status.Name);
                AppliedFilters.Add(filterCriteria);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));
            }
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
                FilterEquipmentItemViewModels = listItems.CreateListItems(AssetDtos.Where(e => e.Category != null && category.Name != null &&
                e.Category.Contains(category.Name, StringComparison.OrdinalIgnoreCase)).ToList());
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

                }
            }
            await Task.Delay(1);
        }

      
        private void OnEquipmentUpdated(AssetDto? dto)
        {
            EquipmentItemViewModel? EquipmentItem = EquipmentItemViewModels?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
           var index = EquipmentItemViewModels?.IndexOf(EquipmentItem!);
            if (EquipmentItem is not null && index != null)
            {
                
                EquipmentItemViewModels![index.Value] = new EquipmentItemViewModel(dto);
                FilterEquipmentItemViewModels![index.Value] = new EquipmentItemViewModel(dto);
            }

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
            newItemViewModel.OpenEquipment = OpenAsset;
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
            IoContainer.Application.GoToPage(ApplicationPage.NewAsset,new NewAssetViewModel { AssetStore = _equipmentStore});
        }
        

        void IterateEquipmentItemsAndChildren(ObservableCollection<EquipmentItemViewModel> equipmentItems)
        {
            foreach (var equipmentItem in equipmentItems)
            {
                equipmentItem.OpenEquipment = OpenAsset;
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children); 
                }
            }
        }

        private async Task OpenAsset(EquipmentItemViewModel? model)
        {
            ViewEquipmentPageViewModel viewEquipment = new()
            {
                AssetStore = _equipmentStore,
                asset = model?.Asset,
                equipmentItem = model,
            };
            IoContainer.Application.GoToPage(ApplicationPage.Equipment, viewEquipment);
            await Task.Delay(1);
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
                    return items.Where(e => e.Category!= null && filter.FilterValue != null && e.Category.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    case EquipmentFilterType.Statut:
                        return items.Where(e => e.Status != null && filter.FilterValue != null && e.Status.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    case EquipmentFilterType.SerialNumber:
                        return items.Where(e => e.SerialNumber != null && filter.FilterValue != null && e.SerialNumber.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    case EquipmentFilterType.Model:
                        return items.Where(e => e.Model != null && filter.FilterValue != null && e.Model.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    case EquipmentFilterType.Make:
                        return items.Where(e => e.Make != null && filter.FilterValue != null && e.Make.Contains(filter.FilterValue, StringComparison.OrdinalIgnoreCase)).ToList();
                    case EquipmentFilterType.DateService:
                        var dateService = filter.FilterValue!.Split(" - ");
                        if (dateService.Length == 2)
                    {
                            var date1 = DateTime.Parse(dateService[0]);
                            var date2 = DateTime.Parse(dateService[1]);
                            return items.Where(e => e.AssetCommissionDate >= date1 && e.AssetCommissionDate <= date2).ToList();
                        }
                        return items;
                    case EquipmentFilterType.DateCreated:
                        var dateCreated = filter.FilterValue!.Split(" - ");
                        if (dateCreated.Length == 2)
                    {
                            var date1 = DateTime.Parse(dateCreated[0]);
                            var date2 = DateTime.Parse(dateCreated[1]);
                            return items.Where(e => e.CreatedDate >= date1 && e.CreatedDate <= date2).ToList();
                        }
                        return items;
                
                default:
                    return items; 
            }
        }
        #endregion









    }
}
