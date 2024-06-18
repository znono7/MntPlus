using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations.Storage;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ShowWorkOrderViewModel : BaseViewModel
    {
        public string? Number { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Priority
        {
            get => _priority;
            set
            {
                if (value == _priority)
                    return;
                _priority = value;
                switch (value)
                {
                    case "1":
                        ForgroundPriority = "c22528";
                        BackgroundPriority = "F8D7DA";
                        WorkPriority = "Priorité Haute";
                        break;
                    case "2":
                        ForgroundPriority = "ef6a00";
                        BackgroundPriority = "FDEBD0";
                        WorkPriority = "Priorité Moyenne";
                        break;
                    case "3":
                        ForgroundPriority = "429b1f";
                        BackgroundPriority = "D0E9C6";
                        WorkPriority = "Priorité Basse";
                        break;
                        case "Aucune" :
                        ForgroundPriority = "29224F";
                        BackgroundPriority = "F0F0F0";
                        WorkPriority = "Aucune Priorité";
                        break;
                }
                OnPropertyChanged(nameof(Priority));
            }
        }

        public string StatusBorderBrush { get; set; } = "429b1f";
        private string? _status { get; set; }
        public string? Status
        {
            get => _status; 
            set
            {
                if (value == _status)
                    return;
                _status = value;
                switch (value)
                {
                    case "Approuvé":
                        ForgroundColorStat = "1C62B9";
                        StatusBorderBrush = "1C62B9";
                        break;
                    case "En attente":
                        ForgroundColorStat = "7B68EE";
                        StatusBorderBrush = "7B68EE";
                        break;
                    case "En service":
                        ForgroundColorStat = "ef6a00";
                        StatusBorderBrush = "ef6a00";
                        break;
                    case "Complet":
                        ForgroundColorStat = "c22528";
                        StatusBorderBrush = "c22528";
                        break;
                    case "Ouvrir":
                        ForgroundColorStat = "429b1f";
                        StatusBorderBrush = "429b1f";
                        break;
                    case "Non spécifique":
                        ForgroundColorStat = "B3B3B3";
                        StatusBorderBrush = "B3B3B3";
                        break;
                }
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand EditWorkOrderCommand { get; set; }
        public Func<WorkOrderDto, Task>? EditAction { get; set; }

        public ICommand DeleteWorkOrderCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }
        public RelayCommand CloseCommand { get;  set; }
        public string? AssignedTo { get; set; }
        public WorkOrderDto? WorkOrder { get; set; }
        public ICommand OpenMenuStatCommand { get; set; }
        public ICommand ApprovedCommand { get; set; }
        public bool IsMenuStatOpen { get;  set; }
        public ICommand PendingCommand { get; set; }
        public ICommand OpenStatCommand { get; set; }
        public ICommand InServiceCommand { get; set; }
        public ICommand CompleteCommand { get; set; }
        public ICommand NonSpecificCommand { get; set; }
        public string ForgroundColorStat { get; set; } = "429b1f";
        public string ForgroundPriority { get; set; } = "29224F";
        public string BackgroundPriority { get; set; } = "7B68EE";
        private string? _priority { get; set; }
        public string WorkPriority { get; set; } = "Basse";

        public string? Location { get; set; }
        public string? Equipment { get; set; }
        public string? Created { get; set; }
        public string? Requester { get; set; }
        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }

        public ObservableCollection<CommentControlViewModel>? Comments { get; set; } 
        public ObservableCollection<WorkOrderHistoryDto>? CommentsHistory { get; set; }
        public WorkOrderStore? WorkOrderStore { get; set; }
        public string? CommentAdded { get; set; } 
        public ShowWorkOrderViewModel(WorkOrderDto? workOrder , WorkOrderStore? workOrderStore)
        {
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            OpenMenuStatCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);
            SaveCommand = new RelayCommand(async () => await AddHistoryAsync());
            DeleteWorkOrderCommand = new RelayCommand(async () => await DeleteWorkOrder());
            WorkOrder = workOrder;
            Number = AddDynamicLeadingZeros(workOrder?.Number ?? 0);
            Name = workOrder?.Name;
            Description = workOrder?.Description;
            Priority = workOrder?.Priority;
            Status  = workOrder?.Status;
            AssignedTo = workOrder?.UserAssignedTo == null ? workOrder?.TeamAssignedTo?.Name : workOrder?.UserAssignedTo?.FullName;
            Location = workOrder?.Asset?.Location != null ? $"{workOrder?.Asset?.Location?.Name}" : null;
            Equipment = workOrder?.Asset != null ? $"{workOrder?.Asset?.Name}" : "";
            Created = $"{workOrder?.CreatedOn?.ToString("dd/MM/yyyy")} ,Par: {workOrder?.UserCreatedBy?.FullName}";
            Requester = workOrder?.Requester;
            ApprovedCommand = new RelayCommand(async () => await UpdateStat("Approuvé"));
            PendingCommand = new RelayCommand(async () => await UpdateStat("En attente"));
            InServiceCommand = new RelayCommand(async () => await UpdateStat("En service"));
            CompleteCommand = new RelayCommand(async () => await UpdateStat("Complet"));
            OpenStatCommand = new RelayCommand(async () => await UpdateStat("Ouvrir"));
            NonSpecificCommand = new RelayCommand(async () => await UpdateStat("Non spécifique"));
            Comments = new ObservableCollection<CommentControlViewModel>();
            _ = GetCommentsHistory();
            Comments = GenerateComments(CommentsHistory);
            WorkOrderStore = workOrderStore;
            EditWorkOrderCommand = new RelayCommand(async () => await EditWorkOrder());


        }

        private async Task EditWorkOrder()
        {
            if (EditAction != null)
                await EditAction(WorkOrder!);
        }

        private async Task DeleteWorkOrder()
        {
            var Dialog = new ConfirmationWindow("Supprimer ordre de travail", "Voulez-vous vraiment supprimer cet ordre de travail ?");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
            {
                return;
            } 
            var Response = await AppServices.ServiceManager.WorkOrderService.DeleteWorkOrder(WorkOrder!.Id, true);
            if (Response is not null && Response is ApiOkResponse<WorkOrderDto> response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Ordre de travail supprimé avec succès"));
                WorkOrderStore?.DeleteWorkOrder(response.Result);
                await CloseAsync();
            }
            else if(Response is not null && Response is ApiBadRequestResponse apiBadRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, apiBadRequestResponse.Message ));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'ordre de travail"));
            }
        }

        private async Task AddHistoryAsync()
        {
            if (string.IsNullOrEmpty(CommentAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le commentaire ne peut pas être vide"));
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                WorkOrderHistoryCreateDto historyCreateDto = new
                    (
                                           Notes: CommentAdded,
                                           Status: Status,
                                           DateChanged: DateTime.Now,
                                           ChangedById: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                                           WorkOrderId: WorkOrder?.Id);

                var Response = await AppServices.ServiceManager.WorkOrderHistoryService.CreateWorkOrderHistory(historyCreateDto);
                if (Response is not null && Response is ApiOkResponse<WorkOrderHistoryDto> response)
                {
                   _ = GetCommentsHistory();
                    Comments = GenerateComments(CommentsHistory);
                    CommentAdded = "";
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de l'ajout du commentaire"));
                }
            });
            
        }

        private async Task GetCommentsHistory()
        {
            if (WorkOrder == null) 
                return;
            var Response = await AppServices.ServiceManager.WorkOrderHistoryService.GetAllWorkOrderHistoriesAsync(WorkOrder.Id,false);
            if (Response is not null && Response is ApiOkResponse<IEnumerable<WorkOrderHistoryDto>> response)
            {
                CommentsHistory = new ObservableCollection<WorkOrderHistoryDto>(response.Result!);
            }
            else if(Response is not null && Response is ApiNotFoundResponse apiNotFoundResponse)
            {
                CommentsHistory = new ObservableCollection<WorkOrderHistoryDto>();
                Comments = new ObservableCollection<CommentControlViewModel>();
            }
            else if(Response is not null && Response is ApiBadRequestResponse apiBadRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, apiBadRequestResponse.Message + "Erreur lors de la récupération de l'historique des commentaires"));
            }else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la récupération de l'historique des commentaires"));
            }
           
           
        }

        private ObservableCollection<CommentControlViewModel>? GenerateComments(ObservableCollection<WorkOrderHistoryDto>? _commentsHistory)
        {
            if (_commentsHistory == null)
                return null;
            var _Comments = new ObservableCollection<CommentControlViewModel>();
            foreach (var comment in _commentsHistory)
            {
                _Comments.Add(new CommentControlViewModel(comment) { RemoveItemFunc = RemoveComment});
            }
            return _Comments;
        }

        private async Task RemoveComment(CommentControlViewModel model)
        {
            var Dialog = new ConfirmationWindow("Supprimer commentaire", "Voulez-vous vraiment supprimer ce commentaire ?");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
            {
                return;
            }
            var Response = await AppServices.ServiceManager.WorkOrderHistoryService.DeleteWorkOrderHistory(model!.WorkOrderHistoryDto!.Id, true);
            if (Response is not null && Response is ApiOkResponse<WorkOrderHistoryDto> response)
            {
                CommentsHistory ??= new ObservableCollection<WorkOrderHistoryDto>();
                CommentsHistory.Remove(response.Result!);
                Comments ??= new ObservableCollection<CommentControlViewModel>();
                Comments.Remove(model);
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression du commentaire"));
            }
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
                await CloseAction();

        }

        private async Task UpdateStat(string status)
        {
            if (WorkOrder == null)
                return;
            
            var Response = await AppServices.ServiceManager.WorkOrderService.UpdateStatWorkOrder(WorkOrder.Id, status, true);
          
            if (Response is not null && Response is ApiOkResponse<WorkOrderDto> response)
            {
                WorkOrder = response.Result;
                Status = status;
                WorkOrderStore?.UpdateWorkOrder(response.Result);

            }
            else if(Response is not null && Response is ApiBadRequestResponse apiBadRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, apiBadRequestResponse.Message ));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la mise à jour du statut de la commande de travail"));
            }
            
        }


        public string AddDynamicLeadingZeros(int number)
        {
            // Get the number of digits in the number
            int numberOfDigits = number.ToString().Length;

            // Calculate the total length after adding zeros
            int totalLength = numberOfDigits * 2 + 1;

            // Pad the number with leading zeros
            return number.ToString().PadLeft(totalLength, '0');
        }
    }
}
