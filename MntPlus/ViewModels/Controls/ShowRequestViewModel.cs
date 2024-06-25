using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ShowRequestViewModel : BaseViewModel
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
                    case "Aucune":
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
                        ForgroundColorStat = "4CAF50";
                        StatusBorderBrush = "4CAF50";
                        break;
                    case "En Attente":
                        ForgroundColorStat = "7B68EE";
                        StatusBorderBrush = "7B68EE";
                        break;
                    case "Refuser":
                        ForgroundColorStat = "ef6a00";
                        StatusBorderBrush = "ef6a00";
                        break;
                   
                }
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand EditReqCommand { get; set; }
        public Func<RequestDto, Task>? EditAction { get; set; }

        public ICommand DeleteReqCommand { get; set; }
        public Func<Task>? CloseAction { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public string? AssignedTo { get; set; }
        public RequestDto? RequestDto { get; set; }
        public RequestStore RequestStore { get; }
        public ICommand ApprovedCommand { get; set; }
       
        public ICommand DeclineCommand { get; set; }
        public string ForgroundColorStat { get; set; } = "429b1f";
        public string ForgroundPriority { get; set; } = "29224F";
        public string BackgroundPriority { get; set; } = "7B68EE";
        private string? _priority { get; set; }
        public string WorkPriority { get; set; } = "Basse";

        public string? Location { get; set; }
        public string? Equipment { get; set; }
        public string? Created { get; set; }
        public string? Requester { get; set; }
       

        public ObservableCollection<CommentControlViewModel>? Comments { get; set; }
        public ObservableCollection<WorkOrderHistoryDto>? CommentsHistory { get; set; }
         
        public bool IsApproved { get; set; }

        public ShowRequestViewModel(RequestDto? requestDto , RequestStore requestStore)
        {
            RequestDto = requestDto;
            RequestStore = requestStore;
            Number = AddDynamicLeadingZeros(requestDto?.Number ?? 0);
            Name = requestDto?.Name;
            Description = requestDto?.Description;
            Priority = requestDto?.Priority;
            Status = requestDto?.Status;
            AssignedTo = requestDto?.UserAssignedTo == null ? requestDto?.TeamAssignedTo?.Name : requestDto?.UserAssignedTo?.FullName;
            Location = requestDto?.Asset?.Location != null ? $"{requestDto?.Asset?.Location?.Name}" : null;
            Equipment = requestDto?.Asset != null ? $"{requestDto?.Asset?.Name}" : "";
            Created = $"{requestDto?.CreatedOn?.ToString("dd/MM/yyyy")} ,Par: {requestDto?.UserCreatedBy?.FullName}";
            Requester = requestDto?.Requester;
            Comments = new ObservableCollection<CommentControlViewModel>();
            CommentsHistory = new ObservableCollection<WorkOrderHistoryDto>();
            IsApproved = requestDto?.Status == "En Attente";
            ApprovedCommand = new RelayCommand(async () => await UpdateStat("Approuvé"));
            DeclineCommand = new RelayCommand(async () => await UpdateStat("Refuser"));
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            EditReqCommand = new RelayCommand(async () => await EditRequest());
            DeleteReqCommand = new RelayCommand(async () => await DeleteRequest());
            GenerateComments();
        }

        private async Task DeleteRequest()
        {
            var Dialog = new ConfirmationWindow("Supprimer demande", "Voulez-vous vraiment supprimer cet demande ?");
            Dialog.ShowDialog();
            if (!Dialog.Confirmed)
            {
                return;
            }
            var Response = await AppServices.ServiceManager.RequestService.DeleteRequest(RequestDto!.Id, true);
            if (Response is not null && Response is ApiOkResponse<string> response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Demande supprimé avec succès"));
                RequestStore?.RemoveRequest(RequestDto);
                await CloseAsync();
            }
            else if (Response is not null && Response is ApiBadRequestResponse apiBadRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, apiBadRequestResponse.Message));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de demande"));
            }
        }

        private async Task CloseAsync()
        {
            if (CloseAction != null)
            {
                await CloseAction();
            }
        }

        private async Task EditRequest()
        {
            if (EditAction != null)
            {
                await EditAction(RequestDto!);
            }
        }

        public bool SaveIsRunning { get; set; }
        private async Task UpdateStat(string status)
        {
            if (RequestDto == null)
                return;
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var Response = await AppServices.ServiceManager.RequestService.UpdateStatRequest(RequestDto.Id, status, true);

                if (Response is not null && Response is ApiOkResponse<RequestDto> response)
                {
                    await Task.Delay(3000);
                    RequestDto = response.Result;
                    Status = status;
                    RequestStore?.UpdateRequest(response.Result);
                    IsApproved = false;
                    Comments?.Add(new CommentControlViewModel($"Demande {status}", RequestDto?.UserCreatedBy?.FullName, DateTime.Now));
                    Comments = new ObservableCollection<CommentControlViewModel>(Comments!.OrderByDescending(x => x.DateChanged));
                }
                else if (Response is not null && Response is ApiBadRequestResponse apiBadRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, apiBadRequestResponse.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la mise à jour du statut de la Demande"));
                }
            });
           
        }

        private string? AddDynamicLeadingZeros(int number)
        {
            // Get the number of digits in the number
            int numberOfDigits = number.ToString().Length;

            // Calculate the total length after adding zeros
            int totalLength = numberOfDigits * 2 + 1;

            // Pad the number with leading zeros
            return number.ToString().PadLeft(totalLength, '0');
        }




        private void GenerateComments()
        {
            if (RequestDto == null)
            {
                // Handle null RequestDto scenario if needed
                return;
            }

            Comments ??= new ObservableCollection<CommentControlViewModel>();

            var createdBy = RequestDto.UserCreatedBy?.FullName;
            var createdOn = RequestDto.CreatedOn;

            Comments.Add(new CommentControlViewModel("Demande Créé", createdBy, createdOn));
            Comments.Add(new CommentControlViewModel("Demande En Attente", createdBy, createdOn));

            if (RequestDto.Status != "En Attente")
            {
                Comments.Add(new CommentControlViewModel($"Demande {RequestDto?.Status}", createdBy, RequestDto?.LastUpdated));
            }

            Comments = new ObservableCollection<CommentControlViewModel>(Comments.OrderByDescending(x => x.DateChanged));
        }

    }
}
