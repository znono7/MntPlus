using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class DueDateFilterViewModel : BaseViewModel
    {
        public bool FromDateMenuIsOpen { get; set; }
        public bool ToDateMenuIsOpen { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateText => FromDate.ToString("dd/MM/yyyy");
        public string ToDateText => ToDate.ToString("dd/MM/yyyy");

        public ICommand OpenFromDateMenuCommand { get; set; }
        public ICommand OpenToDateMenuCommand { get; set; }

        public DueDateFilterViewModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            OpenFromDateMenuCommand = new RelayCommand(() => FromDateMenuIsOpen = !FromDateMenuIsOpen);
            OpenToDateMenuCommand = new RelayCommand(() => ToDateMenuIsOpen = !ToDateMenuIsOpen);
        }
    }
}
