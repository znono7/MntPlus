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
    public class AssetChildrenViewModel : BaseViewModel
    {
        protected string mLastSearchText;
        protected string mSearchText;

        public ICommand SearchCommand { get; set; }
        public ICommand AddNewChildCommand { get; set; }

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

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || Children is null || Children.Count <= 0)
            {
                // Make filtered list the same
                FilteredChildren = new ObservableCollection<AssetItemViewModel>(Children ?? Enumerable.Empty<AssetItemViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }

            FilteredChildren = new ObservableCollection<AssetItemViewModel>(SearchItems(Children, SearchText));
            // Set last search text
            mLastSearchText = SearchText;
        }

        private ObservableCollection<AssetItemViewModel>? _children;
        public AssetStore equipmentStore { get; set; }
        public Guid? Parent { get; set; }
        public AssetChildrenViewModel(Guid? parent ,AssetStore baseequipmentStore,ObservableCollection<AssetItemViewModel>? _children)
        {
            SearchCommand = new RelayCommand(Search);
            AddNewChildCommand = new RelayCommand(AddNewChild);
            equipmentStore = new AssetStore();
            equipmentStore.AssetCreated += EquipmentStore_EquipmentCreated;
           
                
            Children = new ObservableCollection<AssetItemViewModel>();
            
            BaseequipmentStore = baseequipmentStore;
            Children = _children;
            Parent = parent;
            foreach (var child in Children)
            {
                child.WidthControl = 640;
            }
            //    child.PropertyChanged += (sender, e) =>
            //    {
            //        if (e.PropertyName == nameof(EquipmentItemViewModel.IsSelected))
            //        {
            //            OnPropertyChanged(nameof(Children));
            //        }
            //    };
            //}
        }

        private void EquipmentStore_EquipmentCreated(AssetDto? dto)
        {
            if (dto is null)
                return;

            var viewModel = new AssetItemViewModel(dto);
            //viewModel.WidthControl = 640;
            Children.Add(viewModel);
            FilteredChildren.Add(viewModel);
            
        }

        private void AddNewChild()
        {
            InitialAssetViewModel viewModel = new InitialAssetViewModel(BaseequipmentStore, Parent, equipmentStore);
            var window = new InitialEquipmentWindow();
            window.DataContext = viewModel;
          
            window.ShowDialog();
        }

        public ObservableCollection<AssetItemViewModel>? FilteredChildren { get;  set; }
        public AssetStore BaseequipmentStore { get; }

        public ObservableCollection<AssetItemViewModel>? Children 
        {
            get { return _children; }
            set
            {
                _children = value;
                FilteredChildren = 
                    new ObservableCollection<AssetItemViewModel>(_children);
                OnPropertyChanged(nameof(Children));
            }
        }

        public IEnumerable<AssetItemViewModel> SearchItems(ObservableCollection<AssetItemViewModel> items, string searchPattern)
        {
            // Case 1: Search for items that end with the word
            if (searchPattern.StartsWith("*") && !searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimStart('*');
                return items.Where(item => item.AssetName.EndsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
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
