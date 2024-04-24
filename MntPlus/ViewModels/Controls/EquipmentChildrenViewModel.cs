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
    public class EquipmentChildrenViewModel : BaseViewModel
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
                FilteredChildren = new ObservableCollection<EquipmentItemViewModel>(Children ?? Enumerable.Empty<EquipmentItemViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }

            FilteredChildren = new ObservableCollection<EquipmentItemViewModel>(SearchItems(Children, SearchText));
            // Set last search text
            mLastSearchText = SearchText;
        }

        private ObservableCollection<EquipmentItemViewModel>? _children;
        public EquipmentStore equipmentStore { get; set; }
        public Guid? Parent { get; set; }
        public EquipmentChildrenViewModel(Guid? parent ,EquipmentStore baseequipmentStore,ObservableCollection<EquipmentItemViewModel>? _children)
        {
            SearchCommand = new RelayCommand(Search);
            AddNewChildCommand = new RelayCommand(AddNewChild);
            equipmentStore = new EquipmentStore();
            equipmentStore.EquipmentCreated += EquipmentStore_EquipmentCreated;
           
                
            Children = new ObservableCollection<EquipmentItemViewModel>();
            
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

        private void EquipmentStore_EquipmentCreated(EquipmentDto? dto)
        {
            if (dto is null)
                return;

            var viewModel = new EquipmentItemViewModel(dto);
            //viewModel.WidthControl = 640;
            Children.Add(viewModel);
            FilteredChildren.Add(viewModel);
            
        }

        private void AddNewChild()
        {
            InitialEquipmentViewModel viewModel = new InitialEquipmentViewModel(BaseequipmentStore, Parent, equipmentStore);
            var window = new InitialEquipmentWindow();
            window.DataContext = viewModel;
          
            window.ShowDialog();
        }

        public ObservableCollection<EquipmentItemViewModel>? FilteredChildren { get;  set; }
        public EquipmentStore BaseequipmentStore { get; }

        public ObservableCollection<EquipmentItemViewModel>? Children 
        {
            get { return _children; }
            set
            {
                _children = value;
                FilteredChildren = 
                    new ObservableCollection<EquipmentItemViewModel>(_children);
                OnPropertyChanged(nameof(Children));
            }
        }

        public IEnumerable<EquipmentItemViewModel> SearchItems(ObservableCollection<EquipmentItemViewModel> items, string searchPattern)
        {
            // Case 1: Search for items that end with the word
            if (searchPattern.StartsWith("*") && !searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimStart('*');
                return items.Where(item => item.EquipmentName.EndsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            // Case 2: Search for items that start with the word
            else if (!searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimEnd('*');
                return items.Where(item => item.EquipmentName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            // Case 3: Search for items that contain the word
            else if (searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.Trim('*');
                return items.Where(item => item.EquipmentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Invalid search pattern
                return Enumerable.Empty<EquipmentItemViewModel>();
            }
        }
    }

   
}
