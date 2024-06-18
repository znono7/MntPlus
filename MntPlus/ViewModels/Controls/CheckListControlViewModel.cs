using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class CheckListControlViewModel : BaseViewModel
    {
        public ObservableCollection<TaskRecord> CheckListItems { get; set; }
        private int CheckListId = 1;
       public bool SaveIsRunning { get; set; } = false;
        public ICommand AddItemCommand { get; set;}
        public ICommand SaveCommand { get; set; } 
        public ICommand RemoveTaskCommand { get; set; }

        public string? CheckListName { get; set; }
        public string? Description { get; set; }
        public CheckListStore? CheckListStore { get; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }

        public string? Title { get; set;} 

        public CheckListControlViewModel(CheckListStore? checkListStore)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());

            CheckListItems = new ObservableCollection<TaskRecord>();
            AddItemCommand = new RelayCommand(() => AddCheckListItem(""));
            SaveCommand = new RelayCommand(async() => await Save());
            RemoveTaskCommand = new RelayParameterizedCommand((p) => RemoveTaskFromList(p));
            CheckListStore = checkListStore;
            Title = "Ajouter une liste de contrôle";
        }
       
      
        private async Task CloseAsync()
        {
            if (CloseAction != null)
            {
                await CloseAction();
            }
        }

        private void RemoveTaskFromList(object? p)
        {
            if (p == null)
                return;
            var task = p as TaskRecord;
            CheckListItems.Remove(task!);
        }
      
        private async Task Save()
        {
            if (string.IsNullOrEmpty(CheckListName))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel( NotificationType.Error, "Le nom de la liste de contrôle est requis"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var checkList = new CreateCheckListDto
                (
                    Name : CheckListName,
                    Description : Description
                );
                var checkListItems = CheckListItems.Select(x => new CreateCheckListItemDto(x.CheckListItemDto.Order, 
                    Description: x.CheckListItemDto.Description, CheckListId: null, IndividualTaskId: null)).ToList();
                var response = await AppServices.ServiceManager.TaskListService.CreateCheckListItems(checkList,checkListItems);
                if(response.Success && response is ApiOkResponse<CheckListDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Liste de contrôle créée avec succès"));
                    CheckListStore?.CreateCheckList(result.Result!);
                    CloseCommand.Execute(null);
                }
                else if(response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la création de la liste de contrôle"));
                }
            });
        }

        public void AddCheckListItem(string text)
        {
            CheckListItems.Add(new TaskRecord(new CheckListItemDto(Guid.Empty, CheckListId++, text)));
        }

       

    }

   
}
