using Entities;
using Entities.Responses.Equipment;
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

        //private ObservableCollection<EquipmentItemViewModel>? _equipmentItems { get; set; }
        //public ObservableCollection<EquipmentItemViewModel>? EquipmentItems
        //{
        //    get { return _equipmentItems; }
        //    set
        //    {
        //        if (_equipmentItems == value)
        //            return;
        //        _equipmentItems = value;
              
        //        OnPropertyChanged(nameof(EquipmentItems));
        //    }
        //}
        //public ObservableCollection<EquipmentItemViewModel> Equipments { get; set; }

        public ObservableCollection<EquipmentItemViewModel>? FilterEquipmentTreeViewItems { get; set; }


        private ObservableCollection<EquipmentItemViewModel>? _equipmentTreeViewItems;
        public ObservableCollection<EquipmentItemViewModel>? EquipmentTreeViewItems
        {
            get { return _equipmentTreeViewItems; }
            set
            {
                if (_equipmentTreeViewItems == value)
                    return;
                _equipmentTreeViewItems = value;
                FilterEquipmentTreeViewItems = new ObservableCollection<EquipmentItemViewModel>(_equipmentTreeViewItems);
                OnPropertyChanged(nameof(EquipmentTreeViewItems));
            }
        }


        public ObservableCollection<EquipmentItemViewModel>? FilterEquipmentItems { get; set; }

        private ObservableCollection<EquipmentItemViewModel>? _equipmentListItems;
        public ObservableCollection<EquipmentItemViewModel>? EquipmentListItems
        {
            get { return _equipmentListItems; }
            set
            {
                if (_equipmentListItems == value)
                    return;
                _equipmentListItems = value;
                FilterEquipmentItems = new ObservableCollection<EquipmentItemViewModel>(_equipmentListItems);

                OnPropertyChanged(nameof(EquipmentListItems));
            }
        }





        public ObservableCollection<EquipmentDto> EquipmentDtos { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMenu2Open { get; set; }
        public bool IsHeaderVisible => IsList || IsHierarchy;
        public bool IsList { get; set; }
        public bool IsHierarchy { get; set; }
        public bool IsLoading { get; set; } 
        public bool IsEmpty { get; set; }
        private EquipmentStore _equipmentStore { get; set; }


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
            _equipmentStore = new EquipmentStore();
            _equipmentStore.EquipmentCreated += OnEquipmentCreated;
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
            
            if(EquipmentDtos is not null)
            {
                IsHierarchy = true;
                EquipmentTreeViewItems = CreateTreeViewItems(EquipmentDtos.ToList());
                EquipmentListItems = CreateListItems(EquipmentDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentTreeViewItems);
                IterateEquipmentItemsAndChildren(EquipmentListItems);

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
                    FilterEquipmentItems = new ObservableCollection<EquipmentItemViewModel>(EquipmentListItems ?? Enumerable.Empty<EquipmentItemViewModel>());

                    // Set last search text
                    mLastSearchText = SearchText;

                    return;
                }

                FilterEquipmentItems = new ObservableCollection<EquipmentItemViewModel>( SearchItems(EquipmentListItems, SearchText));
                // Set last search text
                mLastSearchText = SearchText;
            }
           
            else if(IsHierarchy)
            {
                if (string.IsNullOrEmpty(SearchText) || EquipmentTreeViewItems is null || EquipmentTreeViewItems.Count <= 0)
                {
                    // Make filtered list the same
                    FilterEquipmentTreeViewItems = new ObservableCollection<EquipmentItemViewModel>(EquipmentTreeViewItems ?? Enumerable.Empty<EquipmentItemViewModel>());

                    // Set last search text
                    mLastSearchText = SearchText;

                    return;
                }
                FilterEquipmentTreeViewItems = new ObservableCollection<EquipmentItemViewModel>( SearchItems(EquipmentTreeViewItems, SearchText));
                // Set last search text
                mLastSearchText = SearchText;
            }
        }

        private void OnEquipmentCreated(EquipmentDto newEquipment)
        {
            // Update the tree view
            var newItemViewModel = new EquipmentItemViewModel(newEquipment);
           
                
            var parentViewModel = FindParentViewModel(newEquipment.EquipmentParent);
            newItemViewModel.AddChildFunc = AddNewChild;
            newItemViewModel.RemoveItemFunc = RemoveItem;

            if (parentViewModel != null)
                {
                    parentViewModel.Children.Add(newItemViewModel);
                }
                else
                {
                    EquipmentTreeViewItems.Add(newItemViewModel);
                }
            
                EquipmentListItems.Add(newItemViewModel);
            

            
            //EquipmentDtos.Add(dto);
            //var newItem = new EquipmentItemViewModel(dto)
            //{
            //    AddChildFunc = AddNewChild,
            //    RemoveItemFunc = RemoveItem
            //};
            //EquipmentItems.Add(newItem);

            //if (IsList)
            //    EquipmentItems = FlattenHierarchy(EquipmentItems);
            //else if (IsHierarchy)
            //    EquipmentItems = OrganizeData(EquipmentItems);
        }

        private ObservableCollection<EquipmentItemViewModel> FlattenHierarchy(ObservableCollection<EquipmentItemViewModel>? models)
        {
            IsList = true;
            List<EquipmentItemViewModel> flatData = [];
            foreach (var rootItem in models)
            {
                flatData.AddRange(OrganizeDataToList(rootItem));
            }
            return new ObservableCollection<EquipmentItemViewModel>(flatData);
        }
        private List<EquipmentItemViewModel> OrganizeDataToList(EquipmentItemViewModel? equipmentItems)
        {
           
           
            List<EquipmentItemViewModel> flatList = [equipmentItems];
                
            foreach (var child in equipmentItems.Children)
                
            {
                    flatList.AddRange(OrganizeDataToList(child));
                
            }
                 
            
            return flatList;
        }

        public async Task LoadDataAsync()
        {
            IsLoading = true;
            var Result = await AppServices.ServiceManager.EquipmentService.GetAllEquipmentsAsync(false);
            if ( Result.Success && Result is ApiOkResponse<IEnumerable<EquipmentDto>> okResponse)
            {
                EquipmentDtos = new ObservableCollection<EquipmentDto>(okResponse.Result) ;
            }
            else 
            if(Result is AssignorListNotFoundResponse response)
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
      
        private void AddEquipment()
        {
                
            InitialEquipmentWindow window = new();
            InitialEquipmentViewModel model = new(_equipmentStore);
          
            window.DataContext = model;
            window.ShowDialog();
        }

      

        private async Task AddNewChild(EquipmentItemViewModel cmodel)
        {
            InitialEquipmentWindow window = new();
            InitialEquipmentViewModel model = new(_equipmentStore,cmodel.Equipment.Id);
          
            window.DataContext = model;
            window.ShowDialog();
            await Task.Delay(1);
        }
        private async Task RemoveItem(EquipmentItemViewModel cmodel)
        {
            if(cmodel.ChildrenCount > 0 && cmodel.Children.Count > 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Vous ne pouvez pas supprimer cet équipement car il contient des enfants"));
                return;
            }
            var Dialog = new ConfirmationWindow(cmodel.Equipment.EquipmentName);
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
                var response = await AppServices.ServiceManager.EquipmentService.DeleteEquipmentAsync(cmodel.Equipment.Id,false);
                if (response.Success)
                {
                    // Remove the item from the tree view
                    RemoveItemFromTreeView(cmodel);

                    // Remove the item from the list view
                    EquipmentListItems.Remove(cmodel);
                    //EquipmentDtos.Remove(cmodel.Equipment);
                    //EquipmentItems.Remove(cmodel);

                    //if (IsList)
                    //    EquipmentItems = FlattenHierarchy(EquipmentItems);
                    //else if (IsHierarchy)
                    //    EquipmentItems = OrganizeData(EquipmentItems);
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipement Supprimé avec Succès"));
                }else if(response is EquipmentDeleteErrorResponse errorResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.ErrorMessage));
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
                equipmentItem.AddChildFunc = AddNewChild;
                equipmentItem.RemoveItemFunc = RemoveItem;
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children);
                }
            }
        }

        private ObservableCollection<EquipmentItemViewModel> OrganizeData(ObservableCollection<EquipmentItemViewModel>? EquipmentList)
        {
            if ( EquipmentList is null || EquipmentList.Count == 0)
                return new ObservableCollection<EquipmentItemViewModel>();
            
            ObservableCollection<EquipmentItemViewModel> RootItems  = new ObservableCollection<EquipmentItemViewModel>();
            

            Dictionary<Guid, EquipmentItemViewModel> equipmentDictionary = new();

            //foreach (var item in EquipmentList)
            //{
            //    if (item.Children is not null && item.Children.Count > 0)
            //        item.Children = new ObservableCollection<EquipmentItemViewModel>();
            //}
            // Create a dictionary to store equipment items by their EquipmentId
            equipmentDictionary = EquipmentList.ToDictionary(e => e.Equipment.Id);

            // Iterate over the EquipmentList to organize them hierarchically
            foreach (var equipment in EquipmentList)
            {
                // Use the null-coalescing operator to handle null EquipmentParent
                Guid parentId = equipment.Equipment.EquipmentParent ?? Guid.Empty;

                if (equipment.Equipment.EquipmentParent == null)
                {
                    // Root level item
                    RootItems.Add(equipment);
                }
                else
                {
                    // Find the parent item and add the equipment as its child
                    if (equipmentDictionary.TryGetValue(parentId, out var parent))
                    {
                        parent.Children.Add(equipment);
                    }
                }
            }
            foreach (var rootItem in RootItems)
            {
                CalculateChildrenCount(rootItem);
            }

            return RootItems;
        }
        private int CalculateChildrenCount(EquipmentItemViewModel item)
        {
            int childrenCount = item.Children.Count;
            foreach (var child in item.Children)
            {
                childrenCount += CalculateChildrenCount(child);
            }
            item.ChildrenCount = childrenCount;
            return childrenCount;
        }


        private void ConvertToEquipmentItemViewModel(ObservableCollection<EquipmentDto>? equipmentDtos)
        {
            if ( equipmentDtos is null || equipmentDtos.Count == 0)
                return;

            FilterEquipmentItems = new ObservableCollection<EquipmentItemViewModel>();
            foreach (var equipmentDto in equipmentDtos)
            {
                FilterEquipmentItems.Add(new EquipmentItemViewModel(equipmentDto));
            }
            IsEmpty = FilterEquipmentItems.Count == 0;
            IterateEquipmentItemsAndChildren(FilterEquipmentItems);
        }
       
        public override void Dispose()
        {
            _equipmentStore.EquipmentCreated -= OnEquipmentCreated;
            base.Dispose();

        }

        private void CalculateNumberOfChildren(EquipmentItemViewModel parentViewModel, List<EquipmentDto> equipmentData)
        {
            // Calculate the number of children for the parentViewModel
            parentViewModel.ChildrenCount = equipmentData.Count(e => e.EquipmentParent == parentViewModel.Equipment.Id);

            // Recursively calculate the number of children for each child
            foreach (var childViewModel in parentViewModel.Children)
            {
                CalculateNumberOfChildren(childViewModel, equipmentData);
            }
        }
        private ObservableCollection<EquipmentItemViewModel> CreateTreeViewItems(List<EquipmentDto>? equipmentData)
        {
            var treeViewItems = new ObservableCollection<EquipmentItemViewModel>();

            // Assuming root items have null ParentId
            var rootItems = equipmentData.Where(e => e.EquipmentParent == null);

            foreach (var rootItem in rootItems)
            {
                var rootViewModel = new EquipmentItemViewModel(rootItem);
                CreateTreeViewChildren(rootViewModel, equipmentData);

                CalculateNumberOfChildren(rootViewModel, equipmentData);

                
                treeViewItems.Add(rootViewModel);
            }

            return treeViewItems;
        }

        private void CreateTreeViewChildren(EquipmentItemViewModel parentViewModel, List<EquipmentDto> equipmentData)
        {
            var children = equipmentData.Where(e => e.EquipmentParent == parentViewModel.Equipment.Id);

            foreach (var child in children)
            {
                var childViewModel = new EquipmentItemViewModel(child);
                CreateTreeViewChildren(childViewModel, equipmentData);
                parentViewModel.Children.Add(childViewModel);
            }
        }

        private EquipmentItemViewModel FindParentViewModel(Guid? parentId)
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

        private EquipmentItemViewModel FindParentViewModelRecursive(EquipmentItemViewModel viewModel, Guid? parentId)
        {
            if (viewModel.Equipment.Id == parentId)
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

        private ObservableCollection<EquipmentItemViewModel> CreateListItems(List<EquipmentDto> equipmentData)
        {
            // For the list view, simply convert the equipment data to view models
            return new ObservableCollection<EquipmentItemViewModel>(
                                equipmentData.Select(e => new EquipmentItemViewModel(e)));
           
          
        }

        private void RemoveItemFromTreeView(EquipmentItemViewModel itemToRemove)
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
        private EquipmentItemViewModel FindParentViewModel(EquipmentItemViewModel itemToRemove)
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

        private EquipmentItemViewModel FindParentViewModelRecursive(EquipmentItemViewModel viewModel, EquipmentItemViewModel itemToRemove)
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

        public IEnumerable<EquipmentItemViewModel> SearchItems(ObservableCollection<EquipmentItemViewModel> items, string searchPattern)
        {
            // Case 1: Search for items that end with the word
            if (searchPattern.StartsWith("*") && !searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimStart('*');
                return items.Where(item => item.EquipmentName.EndsWith(searchTerm));
            }
            // Case 2: Search for items that start with the word
            else if (!searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimEnd('*');
                return items.Where(item => item.EquipmentName.StartsWith(searchTerm));
            }
            // Case 3: Search for items that contain the word
            else if (searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.Trim('*');
                return items.Where(item => item.EquipmentName.Contains(searchTerm));
            }
            else
            {
                // Invalid search pattern
                return Enumerable.Empty<EquipmentItemViewModel>();
            }
        }
    }
}
