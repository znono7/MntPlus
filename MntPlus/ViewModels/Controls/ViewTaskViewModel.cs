using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class ViewTaskViewModel : BaseViewModel
    {
        public TextEntryViewModel TaskName { get; set; }

        public ViewTaskViewModel()
        {
            TaskName = new TextEntryViewModel { OriginalText = "Task Name" };
        }
    }
}
