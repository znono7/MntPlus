using Entities;
using MntPlus.WPF;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentPageViewModel : BaseViewModel
    {
        #region protected
        protected string mLastSearchText;
        protected string mSearchText;

        #endregion
        #region Public Properties

       
        public ObservableCollection<AssetItemViewModel>? FilterEquipmentTreeViewItems { get; set; }


        private ObservableCollection<AssetItemViewModel>? _equipmentTreeViewItems;
        public ObservableCollection<AssetItemViewModel>? EquipmentTreeViewItems
        {
            get { return _equipmentTreeViewItems; }
            set
            {
                if (_equipmentTreeViewItems == value)
                    return;
                _equipmentTreeViewItems = value;
                FilterEquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>(_equipmentTreeViewItems);
                OnPropertyChanged(nameof(EquipmentTreeViewItems));
            }
        }


        public ObservableCollection<AssetItemViewModel>? FilterEquipmentItems { get; set; }

        private ObservableCollection<AssetItemViewModel>? _equipmentListItems;
        public ObservableCollection<AssetItemViewModel>? EquipmentListItems
        {
            get { return _equipmentListItems; }
            set
            {
                if (_equipmentListItems == value)
                    return;
                _equipmentListItems = value;
                FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>(_equipmentListItems);

                OnPropertyChanged(nameof(EquipmentListItems));
            }
        }





        public ObservableCollection<AssetDto> AssetDtos { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMenu2Open { get; set; }
        public bool IsHeaderVisible => IsList || IsHierarchy;
        public bool IsList { get; set; }
        public bool IsHierarchy { get; set; }
        public bool IsLoading { get; set; } 
        public bool IsEmpty => EquipmentListItems is null || EquipmentListItems.Count == 0 ;
        private AssetStore _equipmentStore { get; set; }


        #endregion

        #region Commands  
        public ICommand MenuCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }   
        public ICommand ImpExpCommand { get; set; }
        public ICommand ToListCommand { get; set; }
        public ICommand TohierarchyCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public string SearchText
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
            //EquipmentListItems = new ObservableCollection<EquipmentItemViewModel>();
            ToListCommand = new RelayCommand(() =>
            {
                IsList = true;
                IsHierarchy = false;
              
            });
            TohierarchyCommand = new RelayCommand(() =>
            {
                IsList = false;
                IsHierarchy = true;
                
            });
            _ = LoadDataAsync();
            
            if(AssetDtos is not null)
            {
                IsHierarchy = true;
                EquipmentTreeViewItems = CreateTreeViewItems(AssetDtos.ToList());
                EquipmentListItems = CreateListItems(AssetDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentTreeViewItems);
                IterateEquipmentItemsAndChildren(EquipmentListItems);

            }


        }

        private void OnEquipmentUpdated(AssetDto? dto)
        {
            if(IsList)
            {
                AssetItemViewModel? item = EquipmentListItems.FirstOrDefault(e => e.Asset.Id == dto.Id);
                
                AssetItemViewModel? itemF = FilterEquipmentItems.FirstOrDefault(e => e.Asset.Id == dto.Id);
                if (item is not null)
                {
                    //int indexItem = EquipmentListItems.IndexOf(item);
                    item.Asset = dto;
                    item.AssetName = dto.Name;
                    item.Description = dto.Description;
                    item.AssetImage = dto.ImagePath;
                }
                if (itemF is not null)
                {
                    itemF.Asset = dto;
                    itemF.AssetName = dto.Name;
                    itemF.Description = dto.Description;
                    itemF.AssetImage = dto.ImagePath;
                    //int indexItemF = FilterEquipmentItems.IndexOf(itemF);
                }
            }
            else if(IsHierarchy)
            {
                AssetItemViewModel? item = EquipmentTreeViewItems.FirstOrDefault(e => e.Asset.Id == dto.Id);
                AssetItemViewModel? itemF = FilterEquipmentTreeViewItems.FirstOrDefault(e => e.Asset.Id == dto.Id);
                if (item is not null)
                {
                    item.Asset = dto;
                    item.AssetName = dto.Name;
                    item.Description = dto.Description;
                    item.AssetImage = dto.ImagePath;
                }
                if (itemF is not null)
                {
                    itemF.Asset = dto;
                    itemF.AssetName = dto.Name;
                    itemF.Description = dto.Description;
                    itemF.AssetImage = dto.ImagePath;
                }
            }
           


            
        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;
            // If we have no search text, or no items
            if (IsList)
            {
                if (string.IsNullOrEmpty(SearchText) || EquipmentListItems is null || EquipmentListItems.Count <= 0)
                {
                    // Make filtered list the same
                    FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>(EquipmentListItems ?? Enumerable.Empty<AssetItemViewModel>());

                    // Set last search text
                    mLastSearchText = SearchText;

                    return;
                }

                FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>( SearchItems(EquipmentListItems, SearchText));
                // Set last search text
                mLastSearchText = SearchText;
            }
           
            else if(IsHierarchy)
            {
                if (string.IsNullOrEmpty(SearchText) || EquipmentTreeViewItems is null || EquipmentTreeViewItems.Count <= 0)
                {
                    // Make filtered list the same
                    FilterEquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>(EquipmentTreeViewItems ?? Enumerable.Empty<AssetItemViewModel>());

                    // Set last search text
                    mLastSearchText = SearchText;

                    return;
                }
                FilterEquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>(SearchItems(EquipmentTreeViewItems, SearchText));
                // Set last search text
                mLastSearchText = SearchText;
            }
        }

        private void OnEquipmentCreated(AssetDto? newEquipment)
        {
            // Update the tree view
            var newItemViewModel = new AssetItemViewModel(newEquipment);
           
                
            var parentViewModel = FindParentViewModel(newEquipment.AssetParent);
            newItemViewModel.AddChildFunc = AddNewChild;
            newItemViewModel.RemoveItemFunc = RemoveItem;
            newItemViewModel.ViewFunc = ViewItem;

            if (parentViewModel != null)
                {
                    parentViewModel.Children.Add(newItemViewModel);
                }
                else
                {
                    EquipmentTreeViewItems.Add(newItemViewModel);
                FilterEquipmentTreeViewItems.Add(newItemViewModel);
                }
            
                EquipmentListItems.Add(newItemViewModel);
            FilterEquipmentItems.Add(newItemViewModel);
            

            
           
        }

        private ObservableCollection<AssetItemViewModel> FlattenHierarchy(ObservableCollection<AssetItemViewModel>? models)
        {
            IsList = true;
            List<AssetItemViewModel> flatData = [];
            foreach (var rootItem in models)
            {
                flatData.AddRange(OrganizeDataToList(rootItem));
            }
            return new ObservableCollection<AssetItemViewModel>(flatData);
        }
        private List<AssetItemViewModel> OrganizeDataToList(AssetItemViewModel? equipmentItems)
        {
           
           
            List<AssetItemViewModel> flatList = [equipmentItems];
                
            foreach (var child in equipmentItems.Children)
                
            {
                    flatList.AddRange(OrganizeDataToList(child));
                
            }
                 
            
            return flatList;
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            var Result = await AppServices.ServiceManager.AssetService.GetAllAssetsAsync(false);
            if ( Result.Success && Result is ApiOkResponse<IEnumerable<AssetDto>> okResponse)
            {
                AssetDtos = new ObservableCollection<AssetDto>(okResponse.Result) ;
            }
            else 
            if(Result is ApiNotFoundResponse response)
            {
                AssetDtos = new ObservableCollection<AssetDto>();
            }
            else if (Result is ApiBadRequestResponse response1)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, response1.Message));
            }
            await Task.Delay(100);
            IsLoading = false;
        }
      
        private void AddEquipment()
        {
                
            InitialEquipmentWindow window = new();
            InitialAssetViewModel model = new(_equipmentStore);
          
            window.DataContext = model;
            window.ShowDialog();
        }

      

        private async Task AddNewChild(AssetItemViewModel cmodel)
        {
            InitialEquipmentWindow window = new();
            InitialAssetViewModel model = new(_equipmentStore,cmodel.Asset.Id);
          
            window.DataContext = model;
            window.ShowDialog();
            await Task.Delay(1);
        }
        private async Task RemoveItem(AssetItemViewModel cmodel)
        {
            if(cmodel.ChildrenCount > 0 && cmodel.Children.Count > 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Vous ne pouvez pas supprimer cet équipement car il contient des enfants"));
                return;
            }
            var Dialog = new ConfirmationWindow(cmodel.Asset.Name);
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
                var response = await AppServices.ServiceManager.AssetService.DeleteAsset(cmodel.Asset.Id,false);
                if (response is ApiOkResponse<AssetDto> && response.Success)
                {
                    // Remove the item from the tree view
                    RemoveItemFromTreeView(cmodel);

                    // Remove the item from the list view
                    EquipmentListItems.Remove(cmodel);
                    
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
       

        void IterateEquipmentItemsAndChildren(ObservableCollection<AssetItemViewModel> equipmentItems)
        {
            foreach (var equipmentItem in equipmentItems)
            {
                equipmentItem.AddChildFunc = AddNewChild;
                equipmentItem.RemoveItemFunc = RemoveItem;
                equipmentItem.ViewFunc = ViewItem;
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children);
                }
            }
        }

        private async Task ViewItem(AssetItemViewModel model)
        {
            EquipmentDataViewModel modelEquip = new(model.Asset , _equipmentStore , model.Children);
            EquipmentDataWindow window = new(modelEquip);
            window.DataContext = modelEquip;
            window.ShowDialog();
            await Task.Delay(1);
        }

        private int CalculateChildrenCount(AssetItemViewModel item)
        {
            int childrenCount = item.Children.Count;
            foreach (var child in item.Children)
            {
                childrenCount += CalculateChildrenCount(child);
            }
            item.ChildrenCount = childrenCount;
            return childrenCount;
        }


        private void ConvertToEquipmentItemViewModel(ObservableCollection<AssetDto>? equipmentDtos)
        {
            if ( equipmentDtos is null || equipmentDtos.Count == 0)
                return;

            FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>();
            foreach (var equipmentDto in equipmentDtos)
            {
                FilterEquipmentItems.Add(new AssetItemViewModel(equipmentDto));
            }
            
            IterateEquipmentItemsAndChildren(FilterEquipmentItems);
        }
       
        public override void Dispose()
        {
            _equipmentStore.AssetCreated -= OnEquipmentCreated;
            base.Dispose();

        }

        private void CalculateNumberOfChildren(AssetItemViewModel parentViewModel, List<AssetDto> equipmentData)
        {
            // Calculate the number of children for the parentViewModel
            parentViewModel.ChildrenCount = equipmentData.Count(e => e.AssetParent == parentViewModel.Asset.Id);

            // Recursively calculate the number of children for each child
            foreach (var childViewModel in parentViewModel.Children)
            {
                CalculateNumberOfChildren(childViewModel, equipmentData);
            }
        }
        private ObservableCollection<AssetItemViewModel> CreateTreeViewItems(List<AssetDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<AssetItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData.Where(e => e.AssetParent == null);

            foreach (var rootItem in rootItems)
            {
                var rootViewModel = new AssetItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);

                CalculateNumberOfChildren(rootViewModel, equipmentData);

                
                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }

        private void CreateTreeViewChildren(AssetItemViewModel parentViewModel, List<AssetDto> equipmentData)
        {
            var children = equipmentData.Where(e => e.AssetParent == parentViewModel.Asset.Id);

            foreach (var child in children)
            {
                var childViewModel = new AssetItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel.Children.Add(childViewModel);
            }
        }

        private AssetItemViewModel FindParentViewModel(Guid? parentId)
        {
            if (parentId == null)
            {
                return null; // No parent for root items
            }

            // Search for the parent view model based on parentId
            foreach (var itemViewModel in EquipmentTreeViewItems)
            {
                var parentViewModel = FindParentViewModelRecursive(itemViewModel, parentId);
                if (parentViewModel != null)
                {
                    return parentViewModel;
                }
            }

            return null; // Parent not found
        }

        private AssetItemViewModel FindParentViewModelRecursive(AssetItemViewModel viewModel, Guid? parentId)
        {
            if (viewModel.Asset.Id == parentId)
            {
                return viewModel;
            }

            foreach (var childViewModel in viewModel.Children)
            {
                var parentViewModel = FindParentViewModelRecursive(childViewModel, parentId);
                if (parentViewModel != null)
                {
                    return parentViewModel;
                }
            }

            return null;
        }

        private ObservableCollection<AssetItemViewModel> CreateListItems(List<AssetDto> equipmentData)
        {
            // For the list view, simply convert the equipment data to view models
            return new ObservableCollection<AssetItemViewModel>(
                                equipmentData.Select(e => new AssetItemViewModel(e)));
           
          
        }

        private void RemoveItemFromTreeView(AssetItemViewModel itemToRemove)
        {
            // Find the parent of the item to remove
            var parentViewModel = FindParentViewModel(itemToRemove);

            if (parentViewModel != null)
            {
                parentViewModel.Children.Remove(itemToRemove);
            }
            else
            {
                EquipmentTreeViewItems.Remove(itemToRemove);
            }
        }
        private AssetItemViewModel FindParentViewModel(AssetItemViewModel itemToRemove)
        {
            // Search for the parent view model of the item to remove
            foreach (var itemViewModel in EquipmentTreeViewItems)
            {
                var parentViewModel = FindParentViewModelRecursive(itemViewModel, itemToRemove);
                if (parentViewModel != null)
                {
                    return parentViewModel;
                }
            }

            return null; // Parent not found
        }

        private AssetItemViewModel FindParentViewModelRecursive(AssetItemViewModel viewModel, AssetItemViewModel itemToRemove)
        {
            // Recursively search for the parent view model of the item to remove
            if (viewModel.Children.Contains(itemToRemove))
            {
                return viewModel;
            }

            foreach (var childViewModel in viewModel.Children)
            {
                var parentViewModel = FindParentViewModelRecursive(childViewModel, itemToRemove);
                if (parentViewModel != null)
                {
                    return parentViewModel;
                }
            }

            return null;
        }

        public IEnumerable<AssetItemViewModel> SearchItems(ObservableCollection<AssetItemViewModel> items, string searchPattern)
        {
            // Case 1: Search for items that end with the word
            if (searchPattern.StartsWith("*") && !searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimStart('*');
                return items.Where(item => item.AssetName.EndsWith(searchTerm,StringComparison.OrdinalIgnoreCase));
            }
            // Case 2: Search for items that start with the word
            else if (!searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimEnd('*');
                return items.Where(item => item.AssetName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            // Case 3: Search for items that contain the word
            else if (searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.Trim('*');
                return items.Where(item => item.AssetName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Invalid search pattern
                return Enumerable.Empty<AssetItemViewModel>();
            }
        }
    }
}
