using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Shared;
using Entities;
using System.Drawing;

namespace MntPlus.WPF
{
    public class EquipmentItemViewModel : BaseViewModel
    {
        #region Public Properties  

        /// <summary>
        /// The display Description of Equipment
        /// </summary>
        public string EquipmentName { get; set; }
        public string? Description { get; set; }
        public string? EquipmentImage { get; set; }
        public EquipmentDto Equipment { get; set; }
        public double WidthControl { get; set; }

        public int ChildrenCount { get; set; }
        public bool IsHaveImage { get; set;}

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
        public ICommand RemoveEquipmentCommand { get; set; }
        public ICommand BrowseCommand { get; set; }
        public ICommand ShowImgCommand { get; set; }
        public ICommand DeleteImgCommand { get; set; }
        public ICommand ViewEquipmentCommand { get; set; }


        public Func<EquipmentItemViewModel, Task> AddChildFunc { get; set; }
        public Func<EquipmentItemViewModel, Task> RemoveItemFunc { get; set; }
        public Func<EquipmentItemViewModel, Task> ViewFunc { get; set; }
        public BitmapImage? MyImageSource { get;  set; }
        #endregion

        #region Constructor
        
        public EquipmentItemViewModel(EquipmentDto equipment, double width = 940)
        {
            
            EquipmentName = equipment.EquipmentName;
            Description = equipment.EquipmentDescription;
            EquipmentImage = equipment.EquipmentNameImage;

            ReadImage(equipment.EquipmentImage);
            Equipment = equipment;
            WidthControl = width;
            BrowseCommand = new RelayCommand(Browse);
            ShowImgCommand = new RelayCommand(ShowImage);
            DeleteImgCommand = new RelayCommand(async () => await RemoveImage());
            AddEquipmentCommand = new RelayCommand(async () => await AddChild());
            RemoveEquipmentCommand = new RelayCommand(async () => await Remove());
            ViewEquipmentCommand = new RelayCommand(async  () => await ViewEquipment());

            Children = new ObservableCollection<EquipmentItemViewModel>();

        }

        private async Task RemoveImage()
        {
            MyImageSource = null;
            EquipmentImage = null;
            IsHaveImage = false;
            var Result = await AppServices.ServiceManager.EquipmentService.UpdateEquipmentImageAsync(Equipment.Id, new EquipmentForImageUpdateDto(EquipmentImage, null), true);
            if (Result is not null && Result is EquipmentDeleteErrorResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
            else if (Result is not null && Result is ApiOkResponse<Equipment> response)
            {
                //Equipment = MapToEquipmentDTO(response.Result)!;
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipment image updated successfully"));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Error updating equipment image"));
            }
        }

        public void ReadImage(byte[]? EquipmentImageBytes)
        {
            if (EquipmentImageBytes != null && EquipmentImageBytes.Length > 0)
            {
                IsHaveImage = true;
                MyImageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(EquipmentImageBytes))
                {
                    stream.Position = 0;
                    MyImageSource.BeginInit();
                    MyImageSource.CacheOption = BitmapCacheOption.OnLoad;
                    MyImageSource.StreamSource = stream;
                    MyImageSource.EndInit();
                }
            }
            
        }
        private async Task ViewEquipment()
        {
            await ViewFunc(this);
            //EquipmentDataViewModel model = new(Equipment); 
            //EquipmentDataWindow window = new(model);
            //window.ShowDialog();
            //await Task.Delay(1);
        }

        private async Task Remove()
        {
            await RemoveItemFunc(this);
        }



        #endregion

        #region Public Methods
        public void ShowImage()
        {
            ShowImageWindow imageWindow = new(MyImageSource);
            imageWindow.ShowDialog();
        }
        public async Task AddChild()
        {
            await AddChildFunc(this);
        }

        public void RemoveChild(EquipmentItemViewModel child)
        {
            Children.Remove(child);
        }

        public void Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            
           
            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
                EquipmentImage = Path.GetFileName(selectedFilePath);
                MyImageSource = new BitmapImage(new Uri(selectedFilePath));
            }
        }
        #endregion
    }
}
