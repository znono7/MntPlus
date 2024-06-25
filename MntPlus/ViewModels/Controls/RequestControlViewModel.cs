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
    public class RequestControlViewModel : AddWorkOrderViewModel
    {
        public ICommand AddRequestCommand { get; set; }
        public RequestStore RequestStore { get; }
        public RequestDto? RequestDto { get; }
        public int RequestNumber { get; set; } = 0;
        public RequestControlViewModel(RequestStore requestStore , RequestDto? requestDto = null)
        {
            AddRequestCommand = new RelayCommand(async () => await AddRequest());
            RequestStore = requestStore;
            RequestDto = requestDto;
            if(requestDto != null)
            {
                IsForUpdate = true;
                Name = requestDto.Name;
                Description = requestDto.Description;
                OrderWorkPriority = requestDto.Priority ?? "";
                StartDate = requestDto.StartDate;
                DueDate = requestDto.DueDate;
                Type = requestDto.Type ?? "";
                Requester = requestDto.Requester ?? "";
                UserGuid = requestDto.UserAssignedToId ?? null;
                TeamGuid = requestDto.TeamAssignedToId ?? null;
                SelectedAsset = requestDto.Asset != null ? new AssetDto(requestDto.Asset.Id, null, null, requestDto.Asset.Name, null, null, null, null, null, null, null, null, null, null, null, null, null, null) : null;
                RequestNumber = requestDto.Number ?? 0;
            }
            else
            {
                GetLastNumber().GetAwaiter().GetResult();

            }
            Title = IsForUpdate ? "Modifier la demande" : "Ajouter une demande";

        }
        private async Task GetLastNumber()
        {
            var r = await AppServices.ServiceManager.RequestService.CreateLastNumberRequest();
            if (r.Success && r is ApiOkResponse<int> result)
            {
                RequestNumber = result.Result;
            }
            Number = AddDynamicLeadingZeros(RequestNumber);

        }
        private async Task AddRequest()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le Nom est obligatoire"));
                return;

            }
            if(IsForUpdate)
            {
                await UpdateRequest();
            }
            else
            {
                await CreateRequest();
            }
           
        }

        private async Task UpdateRequest()
        {
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var request = new RequestForCreationDto
                (Name: Name,
                 Number: RequestNumber,
                 Description: Description,
                 Priority: OrderWorkPriority,
                 StartDate: StartDate,
                 DueDate: DueDate,
                 Type: Type,
                 Status: RequestDto?.Status ?? "",
                 Requester: Requester,
                 CreatedOn: RequestDto?.CreatedOn ?? DateTime.Now,
                 UserCreatedId: RequestDto?.UserCreatedId ?? Guid.Empty,
                 UserAssignedToId: UserGuid == null ? null : UserGuid,
                 TeamAssignedToId: TeamGuid == null ? null : TeamGuid,
                 AssetId: SelectedAsset?.Id == null ? null : SelectedAsset?.Id,
                 LastUpdated: DateTime.Now);
                var response = await AppServices.ServiceManager.RequestService.UpdateRequest(RequestDto?.Id ?? Guid.Empty,request,true);
                if (response.Success && response is ApiOkResponse<RequestDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La demande a été modifiée avec succès"));
                    RequestStore.UpdateRequest(result.Result);
                    //close the dialog
                    CloseCommand.Execute(null);
                }
                else if (response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }else if(response is ApiNotFoundResponse notFoundResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, notFoundResponse.Message));
                }

            });
        }

        private async Task CreateRequest()
        {
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var request = new RequestForCreationDto
                (
                     Name: Name,
                    Number: RequestNumber,
                    Description: Description,
                    Priority: OrderWorkPriority,
                    StartDate: StartDate,
                    DueDate: DueDate,
                    Type: Type,
                    Status: "En Attente",
                    Requester: Requester,
                    CreatedOn: DateTime.Now,
                    UserCreatedId: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                    UserAssignedToId: UserGuid == null ? null : UserGuid,
                    TeamAssignedToId: TeamGuid == null ? null : TeamGuid,
                    AssetId: SelectedAsset?.Id == null ? null : SelectedAsset?.Id,
                    LastUpdated: DateTime.Now
                    );
                var response = await AppServices.ServiceManager.RequestService.CreateRequest(request);
                if (response.Success && response is ApiOkResponse<RequestDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "La demande a été créée avec succès"));
                    RequestStore.CreateRequest(result.Result);
                    CloseCommand.Execute(null);

                }
                else if (response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }

            });
        }
    }
}
