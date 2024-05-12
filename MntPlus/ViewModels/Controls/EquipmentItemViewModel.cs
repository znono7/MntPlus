using Entities;
using Shared;
using System.Collections.ObjectModel;

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
        public EquipmentItemViewModel(AssetDto? asset)
        {
            Asset = asset;
            MapProperties();
            Children = new ObservableCollection<EquipmentItemViewModel>();
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
