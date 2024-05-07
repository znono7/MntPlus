using System;
using System.Collections.Generic;
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
        public FilterAssetControlViewModel()
        {
            CategoryPopupCommand = new RelayCommand(() => IsCategoryPopupOpen = !IsCategoryPopupOpen);
        }
    }
}
