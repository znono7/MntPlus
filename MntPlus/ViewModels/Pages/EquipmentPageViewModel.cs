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
using System.Windows;
using System.Windows.Controls;
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
                FilterEquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>(_equipmentTreeViewItems!);
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
                FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>(_equipmentListItems!);

                OnPropertyChanged(nameof(EquipmentListItems));
            }
        }





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
            EquipmentListItems = new ObservableCollection<AssetItemViewModel>();
            EquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>();
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
            
            if(AssetDtos is not null && AssetDtos.Count > 0)
            {
                CreateEquipmentTreeViewItems treeViewItems = new();
                CreateEquipmentListItems equipmentListItems = new();
                IsHierarchy = true;
                EquipmentTreeViewItems = treeViewItems.CreateTreeViewItems(AssetDtos.ToList());
                EquipmentListItems = equipmentListItems.CreateListItems(AssetDtos.ToList());
                IterateEquipmentItemsAndChildren(EquipmentTreeViewItems);
                IterateEquipmentItemsAndChildren(EquipmentListItems);
                IsHeaderVisible = true;
                IsEmpty = false;
            }
            FilterAssetControl = new FilterAssetControlViewModel();
            OpenFilterCommand = new RelayCommand(() => IsFilterOpen = !IsFilterOpen);


        }

        private void OnEquipmentUpdated(AssetDto? dto)
        {
            if(IsList)
            {
                AssetItemViewModel? item = EquipmentListItems?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
                
                AssetItemViewModel? itemF = FilterEquipmentItems?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
                if (item is not null)
                {
                    item.Asset = dto;
                    item.AssetName = dto?.Name;
                    item.Description = dto?.Description;
                    item.AssetImage = dto?.ImagePath;
                }
                if (itemF is not null)
                {
                    itemF.Asset = dto;
                    itemF.AssetName = dto?.Name;
                    itemF.Description = dto?.Description;
                    itemF.AssetImage = dto?.ImagePath;
                }
            }
            else if(IsHierarchy)
            {
                AssetItemViewModel? item = EquipmentTreeViewItems?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
                AssetItemViewModel? itemF = FilterEquipmentTreeViewItems?.FirstOrDefault(e => e.Asset?.Id == dto?.Id);
                if (item is not null)
                {
                    item.Asset = dto;
                    item.AssetName = dto?.Name;
                    item.Description = dto?.Description;
                    item.AssetImage = dto?.ImagePath;
                }
                if (itemF is not null)
                {
                    itemF.Asset = dto;
                    itemF.AssetName = dto?.Name;
                    itemF.Description = dto?.Description;
                    itemF.AssetImage = dto?.ImagePath;
                }
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
                FilterEquipmentItems = new ObservableCollection<AssetItemViewModel>( searchHelper.SearchItems(EquipmentListItems, SearchText) );
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

                FilterEquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>(searchHelper.SearchItems(EquipmentTreeViewItems, SearchText));
                // Set last search text
                mLastSearchText = SearchText;
            }
        }

        private void OnEquipmentCreated(AssetDto? newEquipment)
        {
            var newItemViewModel = new AssetItemViewModel(newEquipment);
            FindEquipmentParentViewModel findEquipmentParentViewModel = new(EquipmentTreeViewItems);


            var parentViewModel = findEquipmentParentViewModel.FindParentViewModel(newEquipment?.AssetParent);
            newItemViewModel.AddChildFunc = AddNewChild;
            newItemViewModel.RemoveItemFunc = RemoveItem;
            newItemViewModel.ViewFunc = ViewItem;

            if (parentViewModel is not null)
            {
                parentViewModel?.Children?.Add(newItemViewModel);
            }
            else
            {
                EquipmentTreeViewItems?.Add(newItemViewModel);
                FilterEquipmentTreeViewItems?.Add(newItemViewModel);
            }

            EquipmentListItems?.Add(newItemViewModel);
            FilterEquipmentItems?.Add(newItemViewModel);

        }

       
      
        public async Task LoadDataAsync()
        {
            await RunCommandAsync( () => IsLoading , async () =>
            {
                var Result = await AppServices.ServiceManager.AssetService.GetAllAssetsAsync(false); 

                if ( Result.Success && Result is ApiOkResponse<IEnumerable<AssetDto>> okResponse)
                {
                    AssetDtos = new ObservableCollection<AssetDto>(okResponse.Result!);

                    EquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>();

                    EquipmentListItems = new ObservableCollection<AssetItemViewModel>();
                   
                }
                
                if (Result is ApiNotFoundResponse response)
                {
                    IsEmpty = true;
                    AssetDtos = new ObservableCollection<AssetDto>();

                    EquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>();

                    EquipmentListItems = new ObservableCollection<AssetItemViewModel>();
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Liste est Vide"));

                }
                 if (!Result.Success && Result is ApiBadRequestResponse response1)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, response1.Message));

                    EquipmentTreeViewItems = new ObservableCollection<AssetItemViewModel>();

                    EquipmentListItems = new ObservableCollection<AssetItemViewModel>();
                }
            });
           
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
            InitialAssetViewModel model = new(_equipmentStore,cmodel.Asset?.Id);
          
            window.DataContext = model;
            window.ShowDialog();
            await Task.Delay(1);
        }
        private async Task RemoveItem(AssetItemViewModel cmodel)
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
                    RemoveEquipmentItemFromTreeView removeEquipmentItem = new(EquipmentTreeViewItems);
                    // Remove the item from the tree view
                    removeEquipmentItem.RemoveItemFromTreeView(cmodel);

                    // Remove the item from the list view
                    EquipmentListItems?.Remove(cmodel);
                    
                    if (EquipmentListItems?.Count == 0)
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

      


       
        public override void Dispose()
        {
            _equipmentStore.AssetCreated -= OnEquipmentCreated;
            base.Dispose();

        }

      


      
       

       
       

       

       
    }
}
