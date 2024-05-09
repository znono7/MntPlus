using Entities;
using Shared;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class NewEquipmentViewModel : MainWindowViewModel
    {
        public Guid? AssetParent { get; set; }
        public Asset? Parent { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AssetStatuses? AssetStatuses { get; set; }
        public AssetStatus? SelectedAssetStatus { get; set; }
        public string? Status { get; set; }

        public EquipmentCategories? EquipmentCategories { get; set; }
        public EquipmentCategory? SelectedEquipmentCategory { get; set; }
        public string? Category { get; set; }

        public Guid? LocationId { get; set; }
        public Location? Location { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public double? PurchaseCost { get; set; }
        public string? ImagePath { get; set; }
        public byte[]? AssetImage { get; set; }
        public DateTime? AssetCommissionDate { get; set; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("d");
        public DateTime? CreatedDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? ShortPurchaseDate => PurchaseDate?.ToString("d");

        public bool IsMenuDueDateOpen { get; set; }
        public bool IsMenuPurchaseDateOpen { get; set; }

        public ICommand OpenMenuPurchaseDateCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }
        public ICommand BrowseCommand { get; set; }

        public BitmapImage? MyImageSource { get;  set; }
        public bool IsHaveImage { get; set; }
        public ICommand DeleteImgCommand { get; set; }

        public NewEquipmentViewModel(Window window) : base(window)
        {
            TitleHeight = 56;
            AssetStatuses = new AssetStatuses();
            EquipmentCategories = new EquipmentCategories();
            OpenMenuPurchaseDateCommand = new RelayCommand(() => IsMenuPurchaseDateOpen = !IsMenuPurchaseDateOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            BrowseCommand = new RelayCommand(async () => await Browse());
            DeleteImgCommand = new RelayCommand(RemoveImage);

        }

        public async Task Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";


            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
                ImagePath = Path.GetFileName(selectedFilePath);
                MyImageSource = new BitmapImage(new Uri(selectedFilePath));
                AssetImage = File.ReadAllBytes(selectedFilePath);
                IsHaveImage = true;
            }
            await Task.Delay(1);
        }
        private void RemoveImage()
        {
            ImagePath = null;
            MyImageSource = null;
            AssetImage = null;
            IsHaveImage = false;
        }

        
    }
}
