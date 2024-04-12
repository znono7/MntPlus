using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MntPlus.WPF
{
    public class NotificationWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// The content to host inside the Notification window
        /// </summary>
        public Control? Content { get; set; }
    }
}
