using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class CommentControlViewModel : BaseViewModel
    {
        public string? Comment { get; set; }
        public Func<Task> CommitAction { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public bool Working { get; set; }
        public string? Status { get; }
        public UserDto? ChangedBy { get; }
        public Guid? WorkOrderId { get; }

        public CommentControlViewModel(string? Status, UserDto? ChangedBy, Guid? WorkOrderId)
        {
            AddCommand = new RelayCommand(async () => await Add());
            CancelCommand = new RelayCommand(() => CommitAction());
            this.Status = Status;
            this.ChangedBy = ChangedBy;
            this.WorkOrderId = WorkOrderId;
        }

        private async Task Add()
        {
            var result = default(bool);
            await RunCommandAsync(() => Working, async () =>
            {
                var Response = await AppServices.ServiceManager.WorkOrderHistoryService.CreateWorkOrderHistory(
                    new WorkOrderHistoryCreateDto( Status ,DateTime.Now , ChangedBy , Comment , WorkOrderId)
                    );
                
                if (Response is not null && Response is ApiOkResponse<WorkOrderHistoryDto> response)
                {
                    result = true;
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Instruction ajoutée avec succès"));
                }
                else
                {
                    result = false;
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de l'ajout de l'instruction"));

                }

            }).ContinueWith(t =>
            {
                if (result)
                {
                    CommitAction();
                }

            });
        }
    }
}
