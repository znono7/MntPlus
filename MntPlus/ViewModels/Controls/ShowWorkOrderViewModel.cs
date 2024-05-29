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
                }
                OnPropertyChanged(nameof(Priority));
            }
        }
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
                        break;
                    case "En attente":
                        ForgroundColorStat = "7B68EE";
                        break;
                    case "En service":
                        ForgroundColorStat = "ef6a00";
                        break;
                    case "Complet":
                        ForgroundColorStat = "c22528";
                        break;
                    case "Ouvrir":
                        ForgroundColorStat = "429b1f";
                        break;
                    case "Non spécifique":
                        ForgroundColorStat = "B3B3B3";
                        break;
                }
                OnPropertyChanged(nameof(Status));
            }
        }
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
            WorkOrder = workOrder;
            Number = FormatNumberWithLeadingZeros(workOrder?.Number) ;
            Name = workOrder?.Name;
            Description = workOrder?.Description;
            Priority = workOrder?.Priority;
            Status  = workOrder?.Status;
            AssignedTo = workOrder?.UserAssignedTo == null ? workOrder?.TeamAssignedTo?.Name : workOrder?.UserAssignedTo?.FullName;
            Location = $"{workOrder?.Asset?.Location?.Name} , {workOrder?.Asset?.Location?.Address}";
            Equipment = $"{workOrder?.Asset?.Name} , {workOrder?.Asset?.SerialNumber}";
            Created = $"{workOrder?.CreatedOn?.ToString("dd/MM/yyyy")} ,Par: {workOrder?.UserCreatedBy?.FullName}";
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
                                           ChangedById: null,/*IoContainer.CurrentUser.Id*/
                                           WorkOrderId: WorkOrder?.Id);

                var Response = await AppServices.ServiceManager.WorkOrderHistoryService.CreateWorkOrderHistory(historyCreateDto);
                if (Response is not null && Response is ApiOkResponse<WorkOrderHistoryDto> response)
                {
                    CommentsHistory ??= new ObservableCollection<WorkOrderHistoryDto>();
                    CommentsHistory.Add(response.Result!);
                    Comments ??= new ObservableCollection<CommentControlViewModel>();
                    Comments.Add(new CommentControlViewModel(response.Result!) { RemoveItemFunc = RemoveComment });
                    CommentAdded = string.Empty;
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
            var Dialog = new ConfirmationWindow(model?.WorkOrderHistoryDto?.Notes);
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
            {
                return;
            }
            var Response = await AppServices.ServiceManager.WorkOrderHistoryService.DeleteWorkOrderHistory(model!.WorkOrderHistoryDto!.Id, false);
            if (Response is not null && Response is ApiOkResponse<WorkOrderHistoryDto> response)
            {
                CommentsHistory ??= new ObservableCollection<WorkOrderHistoryDto>();
                CommentsHistory.Remove(response.Result!);
                Comments ??= new ObservableCollection<CommentControlViewModel>();
                Comments.Remove(model);
                //Comments = GenerateComments(CommentsHistory);
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
            
           var Response = await AppServices.ServiceManager.WorkOrderService.UpdateWorkOrder(WorkOrder.Id,new WorkOrderForCreationDto
               ( Name, WorkOrder.Number, Description, Priority, WorkOrder.StartDate, WorkOrder.DueDate, WorkOrder.Type, status, WorkOrder.Requester, WorkOrder.CreatedOn, WorkOrder.UserCreatedId, WorkOrder.UserAssignedToId, WorkOrder.TeamAssignedToId, WorkOrder.AssetId)
               , true);
            if (Response is not null && Response is ApiOkResponse<WorkOrderDto> response)
            {
                WorkOrder = response.Result;
                Status = status;
                WorkOrderHistoryCreateDto historyCreateDto = new
                   (
                       Notes: $"l'ordre de travail mis à jour, statut : {status}",
                       Status: status,
                       DateChanged: DateTime.Now,
                       ChangedById: null,//IoContainer.CurrentUser.Id,
                       WorkOrderId: response.Result?.Id
                   );
                await AppServices.ServiceManager.WorkOrderHistoryService.CreateWorkOrderHistory(historyCreateDto);
                WorkOrderStore?.UpdateWorkOrder(response.Result);

            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la mise à jour du statut de la commande de travail"));
            }
            
        }

        public  string FormatNumberWithLeadingZeros(int? number)
        {
            if (number.HasValue)
            {
                return $"#{number.Value.ToString("D6")}";
            }
            return "000000"; // Default value for null
        }
    }
}
