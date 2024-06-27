using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class SelectTaskViewModel : ChecklistsViewModel
    {
        public ICommand SelectCommand { get; set; }
        public ICommand GetSelectedCommand { get; set; }
        public CheckListStore CheckListSelectStore { get; set; }
        public CheckListRecord? SelectedCheckList { get; set; }

        public SelectTaskViewModel(CheckListStore checkListStore)
        {
            SelectCommand = new RelayParameterizedCommand(Select);
            GetSelectedCommand = new RelayParameterizedCommand(GetSelected);
            CheckListSelectStore = checkListStore;

        }

        private void GetSelected(object? obj)
        {
            if (SelectedCheckList == null)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une liste pour continuer"));
                return;
            }            
            if (obj != null && obj is SelectTaskWindow window) 
            {
                CheckListSelectStore.SelectCheckList(SelectedCheckList.CheckListDto);
                window.Close();
            }
        }

        private void Select(object? model)
        {
            if (model != null && model is CheckListRecord checkListRecord)
            {
                SelectedCheckList = checkListRecord;
            }
        }
    }
}
