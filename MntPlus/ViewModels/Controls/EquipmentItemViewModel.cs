using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus
{
    public class EquipmentItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The display Description of Equipment
        /// </summary>
        public string EquipmentName { get; set; }
        public string EquipmentId { get; set; }
        public string? EquipmentParent { get; set; }
        public string? EquipmentCategory { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentImage { get; set; }
         
        public double WidthControl { get; set; }

        public bool IsHaveImage => !string.IsNullOrEmpty(EquipmentImage);

        public bool IsHaveChildren => Children.Count > 0;

        private ObservableCollection<EquipmentItemViewModel> _children;
        public ObservableCollection<EquipmentItemViewModel> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }


        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Public Commands
        public ICommand AddEquipmentCommand { get; set; }

        public Func<EquipmentItemViewModel, Task> AddChildFunc { get; set; }
        #endregion

        #region Constructor
        //generate constructor with all properties
        public EquipmentItemViewModel(string name, string id, string? parent = null, string? category = null, string? model = null, string? make = null, string? image = null, double width = 940)
        {
            EquipmentName = name;
            EquipmentId = id;
            EquipmentParent = parent;
            EquipmentCategory = category;
            EquipmentModel = model;
            EquipmentMake = make;
            EquipmentImage = image;
            WidthControl = width;
            Children = new ObservableCollection<EquipmentItemViewModel>();
            AddEquipmentCommand = new RelayCommand(async () => await AddChild());
        }
          
       
        #endregion

        #region Public Methods
        public async Task AddChild()
        {
            await AddChildFunc(this);
        }

        public void RemoveChild(EquipmentItemViewModel child)
        {
            Children.Remove(child);
        }

       
        #endregion
    }
}
