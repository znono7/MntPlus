using Shared;
using System.IO;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {
        private AssetDto? _asset;
        public AssetDto? Asset
        {
            get => _asset;
            set
            {
                if (value == null)
                    return;
                _asset = value;
                MapData(value);
            }
        }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public double? PurchaseCost { get; set; }
        public string? ImagePath { get; set; }
        private byte[]? _assetImage { get; set; }
        public byte[]? AssetImage 
        { 
            get => _assetImage; 
            set
            {
                _assetImage = value;
                

            }
        }
        public string? AssetCommissionDate { get; set; }
        public string? CreatedDate { get; set; }
        public string? PurchaseDate { get; set; }
        public BitmapImage? MyImageSource { get; private set; }
        public bool IsHaveImage { get;  set; }

        public EquipmentInfoViewModel(AssetDto assetDto)
        {
            Asset = assetDto;
        }
        public void ReadImage(byte[]? Image)
        {
            if (Image != null && Image.Length > 0)
            {
                IsHaveImage = true;

                BitmapImage image = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(Image))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
                MyImageSource = image;
            }
        }

        private void MapData(AssetDto asset)
        {
            Name = asset.Name;
            Description = asset.Description;
            Status = asset.Status;
            Category = asset.Category;
            Location = asset.Location?.Name;
            SerialNumber = asset.SerialNumber;
            Model = asset.Model;
            Make = asset.Make;
            PurchaseCost = asset.PurchaseCost;
            AssetCommissionDate = asset.AssetCommissionDate.HasValue ? asset.AssetCommissionDate.Value.ToShortDateString() : "";
            CreatedDate = asset.CreatedDate.HasValue ? asset.CreatedDate.Value.ToShortDateString() : "";
            PurchaseDate = asset.PurchaseDate.HasValue ? asset.PurchaseDate.Value.ToShortDateString() : "";
            ReadImage(asset.AssetImage);
        }
    }
}
