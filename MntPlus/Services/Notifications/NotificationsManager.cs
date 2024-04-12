using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class NotificationsManager : INotificationsManager
    {
        public Task ShowMessage(NotificationControlViewModel viewModel)
        {
            return new NotificationControl().ShowDialog(viewModel);
            
        }
    }
}
