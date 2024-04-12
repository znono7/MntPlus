using MntPlus.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentPageViewModel : BaseViewModel
    {
        #region Public Properties
        public ObservableCollection<EquipmentItemViewModel> EquipmentItems { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMenu2Open { get; set; }
        public bool IsList { get; set; } = false;
         
        #endregion

        #region Commands  
        public ICommand MenuCommand { get; set; }
        public ICommand AddEquipmentCommand { get; set; }   
        public ICommand ImpExpCommand { get; set; }
        public ICommand ToListCommand { get; set; }
        public ICommand TohierarchyCommand { get; set; }
        #endregion

        public EquipmentPageViewModel()
        {
            MenuCommand = new RelayCommand(() => IsMenuOpen = !IsMenuOpen);
            ImpExpCommand = new RelayCommand(() => IsMenu2Open = !IsMenu2Open);
            AddEquipmentCommand = new RelayCommand(AddEquipment);
            //EquipmentItems = new ObservableCollection<EquipmentItemViewModel>(ConvertToEquipmentItemViewModel(GetDataFromDatabase()));
            //OrganizeData(EquipmentItems);
            ToListCommand = new RelayCommand(() =>
            {
                IsList = true;
                GenerateDataWithoutChildren();
            });
            TohierarchyCommand = new RelayCommand(() =>
            {
                IsList = false;
                GenerateDataWithChildren();
            });
            GenerateDataWithChildren();
        }
        private IEnumerable<EquipmentItem> GetDataFromDatabase()
        {
            // Retrieve data from the database here
            // This is just a mock implementation for demonstration purposes
            return new List<EquipmentItem>
        {
            new EquipmentItem { EquipmentId = "1", EquipmentName = "Equipment 1", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "2", EquipmentName = "Equipment 2", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "3", EquipmentName = "Sub Equipment 1", EquipmentParent = "1" },
            new EquipmentItem { EquipmentId = "4", EquipmentName = "Sub Equipment 2", EquipmentParent = "1" },
            new EquipmentItem { EquipmentId = "5", EquipmentName = "Sub Sub Equipment 1", EquipmentParent = "3" },
            new EquipmentItem { EquipmentId = "6", EquipmentName = "Equipment 3", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "7", EquipmentName = "Equipment 4", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "8", EquipmentName = "Equipment 5", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "9", EquipmentName = "Equipment 6", EquipmentParent = null },
            new EquipmentItem { EquipmentId = "10", EquipmentName = "Equipment 7", EquipmentParent = null }
        };
        }
        private void AddEquipment()
        {
            //if ((new ConfirmationWindow("Equipment 7").ShowDialog() ?? false))
           
           
           InitialEquipmentWindow window = new();
            InitialEquipmentViewModel model = new();
            model.EquipmentAdded += (s, e) =>
            {
                EquipmentItems.Add(new EquipmentItemViewModel(e.AddEquipment.EquipmentName, e.AddEquipment.Id.ToString()));
            };
            window.DataContext = model;
            window.ShowDialog();
        }

        public void GenerateDataWithoutChildren()
        {
            EquipmentItems = new ObservableCollection<EquipmentItemViewModel>()
            {
                new EquipmentItemViewModel(Guid.Empty,"Equipment 1", "1"),
                new EquipmentItemViewModel(Guid.Empty,"Equipment 2", "2"),
                new EquipmentItemViewModel(Guid.Empty,"Equipment 3", "3"),
            };
           
        }

        private async Task AddNewChild(EquipmentItemViewModel cmodel)
        {
            AddEquipmentViewModel model = new AddEquipmentViewModel(cmodel);
            model.EquipmentAdded += (s, e) =>
            {
                cmodel.Children.Add(new EquipmentItemViewModel(e.AddEquipment.EquipmentName,e.AddEquipment.EquipmentId));
            };
            AddEquipmentWindow window = new AddEquipmentWindow
            {
                DataContext = model
            };
            window.ShowDialog();
            await Task.Delay(10);

        }

        public void GenerateDataWithChildren()
        {
            
            EquipmentItems = new ObservableCollection<EquipmentItemViewModel>()
            {
                new EquipmentItemViewModel(Guid.Empty,"Equipment 1", "1"),
                new EquipmentItemViewModel(Guid.Empty,"Equipment 2", "2")
                {
                    Children = new ObservableCollection<EquipmentItemViewModel>()
                    {
                        new EquipmentItemViewModel(Guid.Empty,"Equipment 2.1", "2.1"),
                        new EquipmentItemViewModel(Guid.Empty, "Equipment 2.2", "2.2")
                        {
                            Children = new ObservableCollection<EquipmentItemViewModel>()
                            {
                                new EquipmentItemViewModel(Guid.Empty, "Equipment 2.2.1", "2.2.1"),
                                new EquipmentItemViewModel(Guid.Empty, "Equipment 2.2.2", "2.2.2")
                            }
                        }
                    }
                },
                new EquipmentItemViewModel(Guid.Empty, "Equipment 3", "3")
                {
                    Children = new ObservableCollection<EquipmentItemViewModel>()
                    {
                        new EquipmentItemViewModel(Guid.Empty, "Equipment 3.1", "3.1"),
                        new EquipmentItemViewModel(Guid.Empty, "Equipment 3.2", "3.2")
                        {
                            Children = new ObservableCollection<EquipmentItemViewModel>()
                            {
                                new EquipmentItemViewModel(Guid.Empty, "Equipment 3.2.1", "3.2.1"),
                                new EquipmentItemViewModel( Guid.Empty,"Equipment 3.2.2", "3.2.2")
                            }
                        }
                    }
                }
            };

           // IterateEquipmentItemsAndChildren(equipmentItems: EquipmentItems);
        }
        public double SetWidthControl( int level)
        {
            if (level == 0)
            {
                return 940;
            }
            else
            {
                return 940 - (20 * level);
            }
        }

        void IterateEquipmentItemsAndChildren(IEnumerable<EquipmentItemViewModel> equipmentItems)
        {
            foreach (var equipmentItem in equipmentItems)
            {
                equipmentItem.AddChildFunc = AddNewChild;
                // Check if the current equipmentItem has children
                if (equipmentItem.Children != null && equipmentItem.Children.Any())
                {
                    // Recursively call the method to iterate over the children
                    IterateEquipmentItemsAndChildren(equipmentItem.Children);
                }
            }
        }
        public ObservableCollection<EquipmentItemViewModel> RootItems { get; set; } = new ObservableCollection<EquipmentItemViewModel>();

        private void OrganizeData(ObservableCollection<EquipmentItemViewModel> EquipmentList)
        {
            // Create a dictionary to store equipment items by their EquipmentId
            var equipmentDictionary = EquipmentList.ToDictionary(e => e.EquipmentId);

            // Iterate over the EquipmentList to organize them hierarchically
            foreach (var equipment in EquipmentList)
            {
                if (equipment.EquipmentParent == null)
                {
                    // Root level item
                    RootItems.Add(equipment);
                }
                else
                {
                    // Find the parent item and add the equipment as its child
                    if (equipmentDictionary.TryGetValue(equipment.EquipmentParent, out var parent))
                    {
                        parent.Children.Add(equipment);
                    }
                }
            }

        }

        //from equipment to equipmentItemViewModel observable collection
        private ObservableCollection<EquipmentItemViewModel> ConvertToEquipmentItemViewModel(IEnumerable<EquipmentItem> equipment)
        {
            var equipmentItems = new ObservableCollection<EquipmentItemViewModel>();
            foreach (var e in equipment)
            {
                equipmentItems.Add(new EquipmentItemViewModel(e.EquipmentName, e.EquipmentId, e.EquipmentParent, e.EquipmentCategory, e.EquipmentModel, e.EquipmentMake, e.EquipmentNameImage));
            }
            return equipmentItems;
        }
    }
}
