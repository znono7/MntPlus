using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class FilterAssetControlViewModel : BaseViewModel
    {
        public bool IsCategoryPopupOpen { get; set; } 
        public ICommand CategoryPopupCommand { get; set; }
        public EquipmentCategories? EquipmentCategories { get; set; }
        private EquipmentCategory? selectedEquipmentCategory { get; set; }
        public EquipmentCategory? SelectedEquipmentCategory
        {
            get => selectedEquipmentCategory;
            set
            {
                selectedEquipmentCategory = value;
                FilterByCategoryCommand.Execute(null);
                IsCategoryPopupOpen = false;
                OnPropertyChanged(nameof(SelectedEquipmentCategory));
            }

        }
        public ICommand FilterByCategoryCommand { get; set; }
        public Func<EquipmentCategory, Task>? FilterByCategoryFonction { get; set; }




       

        public FilterAssetControlViewModel()
        {
            CategoryPopupCommand = new RelayCommand(() => IsCategoryPopupOpen = !IsCategoryPopupOpen);
            EquipmentCategories = new EquipmentCategories();
            FilterByCategoryCommand = new RelayCommand(async () => await FilterByCategory());

        }

        private async Task FilterByCategory()
        {
            if(FilterByCategoryFonction != null && SelectedEquipmentCategory != null)
            {
                await FilterByCategoryFonction(this.SelectedEquipmentCategory);

            }
        }
    }
}
