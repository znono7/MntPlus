using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentItemViewModel : BaseViewModel
    {
        public AssetDto? Parent { get; set; }
        public string? Name { get; set; }
        public string? ParentName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } 
        public string? Category { get; set; }
        public LocationDto? Location { get; set; } 
        public string? LocationName { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public double? PurchaseCost { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? AssetImage { get; set; }
        public string? AssetCommissionDate { get; set; }
        public string? CreatedDate { get; set; }
        public string? PurchaseDate { get; set; }
        public AssetDto? Asset { get; set; }
        private bool _isChecked { get; set; }
        public bool IsChecked 
        { 
            get => _isChecked; 
            set 
            { 
                _isChecked = value;
                IterateEquipmentChildren(_children,value);
                OnPropertyChanged(nameof(IsChecked)); 
            } 
        }

        private ObservableCollection<EquipmentItemViewModel>? _children;

        public ObservableCollection<EquipmentItemViewModel>? Children
        {
            get { return _children; }
            set
            {
                _children = value;

                OnPropertyChanged(nameof(Children));
            }
        }
        public int ChildrenCount { get; set; }

        public ICommand ViewEquipmentCommand { get; set; }

        public bool IsHasStatut => Status != null;
        public EquipmentItemViewModel(AssetDto? asset) 
        {
            Asset = asset;
            MapProperties();
            Children = new ObservableCollection<EquipmentItemViewModel>();
            ViewEquipmentCommand = new RelayCommand(ViewEquipment);
        }

        private void ViewEquipment()
        {
            EquipmentInfoViewModel equipment = new()
            {
                Name = Name,
                Description = Description,
                Status = Status,
                Category = Category,
                Location = Location?.Name,
                SerialNumber = SerialNumber,
                Model = Model,
                Make = Make,
                PurchaseCost = PurchaseCost,
                ImagePath = ImagePath,
                AssetImage = AssetImage,
                AssetCommissionDate = AssetCommissionDate,
                CreatedDate = CreatedDate,
                PurchaseDate = PurchaseDate

            };
            // Get and display the full hierarchy
            EquipmentHierarchyManager manager = new EquipmentHierarchyManager();
            manager.GetFullHierarchy(this);
            if(manager.EquipmentHierarchy != null)
                EquipmentHierarchy = new ObservableCollection<AssetItemViewModel>(FromEquipToAsset( manager.EquipmentHierarchy));
            AssetChildrenViewModel assetChildren = new()
            {
                Children = EquipmentHierarchy
            };
            EquipmentMeterViewModel equipmentMeterViewModel = new(Asset!.Id);
            ViewEquipmentPageViewModel viewEquipment = new()
            {
                equipmentInfo = equipment,
                childrenViewModel = assetChildren,
                equipmentMeterViewModel = equipmentMeterViewModel
            }; 
            IoContainer.Application.GoToPage(ApplicationPage.Equipment, viewEquipment);
        }
        public ObservableCollection<AssetItemViewModel> EquipmentHierarchy = new();

        public List< AssetItemViewModel> FromEquipToAsset(List< EquipmentItemViewModel> assetItem)
        {
            List<AssetItemViewModel> assetItems = new();
            foreach (var item in assetItem)
            {
                if(item.Asset?.Id == Asset?.Id)
                {
                    AssetItemViewModel asset = new()
                    {
                        AssetName = $"Cette équipement : {item.Name}"
                    };
                    assetItems.Add(asset);

                }
                else
                {
                    AssetItemViewModel asset = new()
                    {
                        AssetName = item.Name
                    };
                    assetItems.Add(asset);

                }

            }
            return assetItems;
            
        }
        public void GetFullHierarchy()
        {
            GetParentHierarchy(this);
            if (HierarchyParents.Count > 0)
            {
                HierarchyParents.Reverse();
                HierarchyParents.Add(this);
                if(Children != null)
                {
                    foreach (var item in Children)
                    {
                        HierarchyParents.Add(item);
                    }
                }
               
            }
            else
            {
                HierarchyParents.Add(this);
                if (Children != null)
                {
                    foreach (var item in Children)
                    {
                        HierarchyParents.Add(item);
                    }
                }

            }
            EquipmentHierarchy = new ObservableCollection<AssetItemViewModel>(FromEquipToAsset(HierarchyParents));
        }
      
        List<EquipmentItemViewModel> HierarchyParents = new List<EquipmentItemViewModel>();

        private void GetParentHierarchy(EquipmentItemViewModel model)
        {
            if (model.Parent != null)
            {
                HierarchyParents.Add(new EquipmentItemViewModel(model.Parent));
                GetParentHierarchy(new EquipmentItemViewModel(model.Parent));
            }
            
        }

        private void IterateEquipmentChildren(ObservableCollection<EquipmentItemViewModel>? equipments,bool val)
        {
            if(equipments != null && equipments.Count > 0)
            {

                foreach (var item in equipments)
                {

                    item.IsChecked = val;
                    if(item.Children != null && item.Children.Count > 0)
                        IterateEquipmentChildren(item.Children,val);
                }
            }
        }
        //map the properties
        private void MapProperties()
        {
            Parent = Asset.Parent;
            ParentName = Asset.Parent?.Name;
            Name = Asset.Name;
            Description = Asset.Description;
            Status = Asset.Status;
            Category = Asset.Category;
            Location = Asset.Location;
            LocationName = Asset.Location?.Name;
            SerialNumber = Asset.SerialNumber;
            Model = Asset.Model;
            Make = Asset.Make;
            PurchaseCost = Asset.PurchaseCost;
            ImagePath = Asset.ImagePath;
            AssetImage = Asset.AssetImage;
            AssetCommissionDate = Asset.AssetCommissionDate.HasValue ? Asset.AssetCommissionDate.Value.ToString("dd/MM/yyyy") : "";
            CreatedDate = Asset.CreatedDate.HasValue ? Asset.CreatedDate.Value.ToString("dd/MM/yyyy") : "";
            PurchaseDate = Asset.PurchaseDate.HasValue ? Asset.PurchaseDate.Value.ToString("dd/MM/yyyy") : "";
        }
    }
}
