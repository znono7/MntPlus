using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class AddTeamViewModel : BaseViewModel
    {
        public string? Name { get; set; }
      
        public UsersCmbViewModel UsersCmbViewModel { get; set; }


        public bool SaveIsRunning { get; set; }

        public AddTeamViewModel()
        {
            UsersCmbViewModel = new UsersCmbViewModel();
           
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
