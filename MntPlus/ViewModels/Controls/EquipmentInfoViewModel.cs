using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {

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
                ReadImage(value);

            }
        }
        public string? AssetCommissionDate { get; set; }
        public string? CreatedDate { get; set; }
        public string? PurchaseDate { get; set; }
        public BitmapImage? MyImageSource { get; private set; }
        public bool IsHaveImage { get;  set; }

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
    }
}
