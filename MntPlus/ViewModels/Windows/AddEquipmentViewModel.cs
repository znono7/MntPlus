using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MntPlus
{
    public class AddEquipmentViewModel : PropertyValidation
    {
        private ImageSource _imageSource;

        public ImageSource MyImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("MyImageSource");
            }
        }

        public string EquipmentId { get; set; }
        public string? EquipmentParent { get; set; }

        [Required(ErrorMessage = $"\uf06a  Nom de l'équipement Doit être Précisé")]
        public string EquipmentName { get => GetValue(() => EquipmentName); set { SetValue(() => EquipmentName, value); } }
        public string? EquipmentCategory { get; set; }
        public string? EquipmentModel { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand BrowseCommand { get; set; }
        public EquipmentItemViewModel? Parent { get; set; }

        public event EventHandler<EquipmentEventArgs> EquipmentAdded;

        public AddEquipmentViewModel(EquipmentItemViewModel? parent = null) 
        {
            BrowseCommand = new RelayCommand(Browse);
            AddCommand = new RelayCommand(AddEquipment);
            Parent = parent;
        }

        private void AddEquipment()
        {
            if (IsValid)
            {
               
                OnEquipmentAdded(new Equipment
                {
                    EquipmentId = EquipmentId,
                    EquipmentName = EquipmentName,
                    EquipmentParent = EquipmentParent,
                    EquipmentCategory = EquipmentCategory,
                    EquipmentModel = EquipmentModel,
                    EquipmentMake = EquipmentMake,
                    EquipmentNameImage = EquipmentNameImage,
                    EquipmentImage = EquipmentImage
                });
            }
            
        }

        private void Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (dlg.ShowDialog() == true)
            {
                MyImageSource = new BitmapImage(new Uri(dlg.FileName));
            }          
        }

        public void OnEquipmentAdded(Equipment equipment)
        {
            EquipmentAdded?.Invoke(this, new EquipmentEventArgs { AddEquipment = equipment });
        }
    }
}
