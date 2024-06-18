using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class UpdateCheckListControlViewModel : BaseViewModel
    {

        public ObservableCollection<TaskRecord> CheckListItems { get; set; }
        private int CheckListId = 0;
        public bool SaveIsRunning { get; set; } = false;
        public ICommand AddItemCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveTaskCommand { get; set; }

        public string? CheckListName { get; set; }
        public string? Description { get; set; }
        public CheckListStore? CheckListStore { get; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }
        public CheckListDto CheckListDto { get; }

        public string? Title { get; set; }
        public UpdateCheckListControlViewModel(CheckListStore? checkListStore, CheckListDto checkListDto)
        {
            CheckListStore = checkListStore;

            Title = "Modifier la liste de contrôle";
            CloseCommand = new RelayCommand(async () => await CloseAsync());

            CheckListItems = new ObservableCollection<TaskRecord>();
            AddItemCommand = new RelayCommand(() => AddMoreCheckListItem(""));
            SaveCommand = new RelayCommand(async () => await Update());
            RemoveTaskCommand = new RelayParameterizedCommand((p) => RemoveTask(p));
            CheckListDto = checkListDto;
            CheckListName = checkListDto.Name;
            Description = checkListDto.Description;
            _ = GetItems();
            CheckListId = CheckListItems.Max(x => x.CheckListItemDto.Order);
        }

        private async Task GetItems()
        {
            var response = await AppServices.ServiceManager.TaskListService.GetCheckListItems(CheckListDto.Id, false);
            if (response.Success && response is ApiOkResponse<IEnumerable<CheckListItemDto>> result)
            {
                CheckListItems = new ObservableCollection<TaskRecord>(result.Result!.Select(x => new TaskRecord(x)).ToList());

            }
            else if (response.Success && response is ApiNotFoundResponse)
            {
                CheckListItems = new ObservableCollection<TaskRecord>();
            }
        }
        private async Task Update()
        {
            if (string.IsNullOrEmpty(CheckListName))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom de la liste de contrôle est requis"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var checkList = new CreateCheckListDto
                (
                                       Name: CheckListName,
                                                          Description: Description
                                                                         );
                var checkListItems = CheckListItems.Select(x => new CheckListItemDto(x.CheckListItemDto.Id, x.Order, Description: x.Description)).ToList();

                var response = await AppServices.ServiceManager.TaskListService.UpdateCheckListItems(CheckListDto.Id, checkList, checkListItems, true);
                if (response.Success && response is ApiOkResponse<CheckListDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Liste de contrôle mise à jour avec succès"));
                    CheckListStore?.UpdateCheckList(result.Result!);
                    CloseCommand.Execute(null);
                }
                else if (response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la mise à jour de la liste de contrôle"));
                }
            });
        }
        private async Task CloseAsync()
        {
            if (CloseAction != null)
            {
                await CloseAction();
            }
        }

        private void RemoveTask(object? p)
        {
            if (p == null)
                return;

            var dialog = new ConfirmationWindow("Supprimer la tâche", "Êtes-vous sûr de vouloir supprimer cette tâche ?");
            dialog.ShowDialog();
            if (!dialog.Confirmed)
                return;

            var task = p as TaskRecord;
            _ = RemoveTaskFromDb(task!.CheckListItemDto.Id);
        }

        private async Task RemoveTaskFromDb(Guid id)
        {
            var response = await AppServices.ServiceManager.TaskListService.DeleteTaskItem(id, true);
            if (response.Success && response is ApiOkResponse<CheckListItemDto> result)
            { 
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Tâche supprimée avec succès"));
                CheckListItems.Remove(CheckListItems.FirstOrDefault(x => x.CheckListItemDto.Id == result.Result!.Id)!);
                //update order
                CheckListItems = new ObservableCollection<TaskRecord>(CheckListItems.Select((x, i) => new TaskRecord(new CheckListItemDto(x.CheckListItemDto.Id, i + 1, x.Description))).ToList());

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de la tâche"));
            }
        }
        public void AddMoreCheckListItem(string text)
        {
            int x = CheckListId + 1;
            CheckListItems.Add(new TaskRecord(new CheckListItemDto(Guid.Empty, x, text)));
        }


    }
}
