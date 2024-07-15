using Entities;
using Shared;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MntPlus.WPF
{
    public class AddWorkOrderViewModel : BaseViewModel
    {
        public AddWorkOrderViewModel()
        {
            BrowseCommand = new RelayCommand(async () => await Browse());
            DeleteImgCommand = new RelayCommand(RemoveImage);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            OpenMenuStartDateCommand = new RelayCommand(() => IsMenuStartDateOpen = !IsMenuStartDateOpen);
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "1");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "2");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "3");
            NonPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Aucune");
            ClearStartDateCommand = new RelayCommand(() => _startDate = null);
            ClearDueDateCommand = new RelayCommand(() => _dueDate = null);
            BrowseToEquipmentCommand = new RelayCommand(BrowseToEquipment);
            ClearEquipmentCommand = new RelayCommand(() => { SelectedAsset = null; browseToEquipmentVisible = true; });
            BrowseToAssignedCommand = new RelayCommand(BrowseToAssigned);
            ClearAssignedCommand = new RelayCommand(() => { SelectedAssignedTo = null; browseToAssignedVisible = true; });
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CloseCommand = new RelayCommand(async () => await CloseAsync());
           
        }
        public string ForgroundColor { get; set; } = "53667B";
        public bool IsMenuPrioprityOpen { get; set; } 
        private string _orderWorkPriority = "Aucune";
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                _orderWorkPriority = value;
                switch (value)
                {
                    case "1":
                        ForgroundColor = "c22528";
                        IsMenuPrioprityOpen = false;
                        break;
                    case "2":
                        ForgroundColor = "ef6a00";
                        IsMenuPrioprityOpen = false;

                        break;
                    case "3":
                        ForgroundColor = "429b1f";
                        IsMenuPrioprityOpen = false;

                        break;
                        case "Aucune":
                        ForgroundColor = "53667B";
                        IsMenuPrioprityOpen = false;
                        break;
                }
            }
        }
        public ICommand BrowseCommand { get; set; }
        public ICommand DeleteImgCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; } 
        public ICommand OpenMenuStartDateCommand { get; set; }
        public ICommand HighPriorityCommand { get; set; }
        public ICommand MediumPriorityCommand { get; set; }
        public ICommand LowPriorityCommand { get; set; }
        public ICommand NonPriorityCommand { get; set; }
        public ICommand OpenMenuPriorityCommand { get; set; }
        public ICommand ClearStartDateCommand { get; set; }
        public ICommand ClearDueDateCommand { get; set; }

        public BitmapImage? MyImageSource { get; set; }
        public bool IsHaveImage { get;  set; }
        public bool IsMenuDueDateOpen { get; set; }
        public bool IsMenuStartDateOpen { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        private DateTime? _dueDate { get; set;}
        public DateTime? DueDate { get => _dueDate; set  { _dueDate = value; IsMenuDueDateOpen = false; } }
        public string? ShortDueDate => DueDate.HasValue ? DueDate.Value.ToString("dd-MM-yyyy") : null;


        private DateTime? _startDate { get; set; }
        public DateTime? StartDate { get => _startDate; set { _startDate = value; IsMenuStartDateOpen = false; } }
        public string? ShortStartDate => StartDate.HasValue ? StartDate.Value.ToString("dd-MM-yyyy") : null;

        public string Type { get; set; } = "Correctif";

        public bool browseToEquipmentVisible { get; set; } = true;
        public ICommand BrowseToEquipmentCommand { get; set; }
        public ICommand ClearEquipmentCommand { get; set; }
        public AssetDto? SelectedAsset { get; set; }
        public AssetStore? SelectedAssetStore { get; set; }
        public string? SelectedAssignedTo { get; set; }
        public bool browseToAssignedVisible { get; set; } = true;
        public ICommand BrowseToAssignedCommand { get; set; }
        public ICommand ClearAssignedCommand { get; set; }
        public UserTeamStore? UserTeamStore { get; set; }
        public Guid? UserGuid { get; set; } 
        public Guid? TeamGuid { get; set; }

        public bool SaveIsRunning { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Func<Task>? CloseAction { get; set; } 

        public string? Number { get; set; }
        public int num { get; set; }
        public string? Requester { get;  set; }
        public WorkOrderStore WorkOrderStore { get; }
        public WorkOrderDto? WorkOrderDto { get; }

        public bool IsForUpdate { get; set; } = false;  
        public string Title { get; set; } 
        public AddWorkOrderViewModel(WorkOrderStore workOrderStore , WorkOrderDto? workOrderDto = null )
        {
            BrowseCommand = new RelayCommand(async () => await Browse());
            DeleteImgCommand = new RelayCommand(RemoveImage);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            OpenMenuStartDateCommand = new RelayCommand(() => IsMenuStartDateOpen = !IsMenuStartDateOpen);
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "1");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "2");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "3");
            NonPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Aucune");
            ClearStartDateCommand = new RelayCommand(() => _startDate = null);
            ClearDueDateCommand = new RelayCommand(() => _dueDate = null);
            BrowseToEquipmentCommand = new RelayCommand(BrowseToEquipment);
            ClearEquipmentCommand = new RelayCommand(() => { SelectedAsset = null; browseToEquipmentVisible = true; });
            BrowseToAssignedCommand = new RelayCommand(BrowseToAssigned);
            ClearAssignedCommand = new RelayCommand(() => { SelectedAssignedTo = null; browseToAssignedVisible = true; });
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CloseCommand = new RelayCommand(async () => await CloseAsync());
            WorkOrderStore = workOrderStore;
            WorkOrderDto = workOrderDto;
            if (workOrderDto != null) 
            {
                IsForUpdate = true;
                Name = workOrderDto.Name;
                Description = workOrderDto.Description;
                DueDate = workOrderDto.DueDate;
                StartDate = workOrderDto.StartDate;
                OrderWorkPriority = workOrderDto.Priority!;
                Type = workOrderDto.Type!;
                SelectedAsset = workOrderDto.Asset != null ? new AssetDto(workOrderDto.Asset.Id,null,null, workOrderDto.Asset.Name,null,null, null, null, null, null, null, null, null, null, null, null, null, null) : null;
                SelectedAssignedTo = workOrderDto.UserAssignedTo?.FullName ?? workOrderDto.TeamAssignedTo?.Name;
                UserGuid = workOrderDto.UserAssignedTo != null ? workOrderDto.UserAssignedTo?.Id : null;
                TeamGuid = workOrderDto.TeamAssignedTo != null ? workOrderDto.TeamAssignedTo?.Id : null;
                Number = AddDynamicLeadingZeros( workOrderDto.Number ?? 0);
                num = workOrderDto.Number ?? 0;
                Requester = workOrderDto.Requester;



            }
            else
            {
                _ = GetLastNumber();

            }
            Title = IsForUpdate ? "Modifier l'ordre de travail" : "Ajouter un ordre de travail";

        }

        public async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le Nom est obligatoire"));
                return;

            }
            if(DueDate <= StartDate)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de fin doit être supérieure à la date de début"));
                return;
            }
            if(IsForUpdate)
            {
                await UpdateAsync();
                return;
            }
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                WorkOrderForCreationDto workOrderForCreationDto = new
                (
                    Name : Name,
                    Number: num,
                    Description : Description,
                    Priority : OrderWorkPriority, 
                    StartDate : StartDate,
                    DueDate : DueDate,
                    Type : Type,
                    Status : "Non spécifique",
                    Requester : Requester,
                    CreatedOn : DateTime.Now,
                    UserCreatedId : Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                    UserAssignedToId : UserGuid == null ? null : UserGuid,
                    TeamAssignedToId : TeamGuid == null ? null : TeamGuid,
                    AssetId : SelectedAsset?.Id == null ? null : SelectedAsset?.Id
                );
                var Response = await AppServices.ServiceManager.WorkOrderService.CreateWorkOrder(workOrderForCreationDto);
                if (Response.Success && Response is ApiOkResponse<WorkOrderDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Ordre de travail créé avec succès"));
                    WorkOrderStore.CreateWorkOrder(result.Result);
                    CloseCommand.Execute(null);
                }
                else if (Response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }
            });
            
        }
        private async Task GetLastNumber()
        {
            var res = await AppServices.ServiceManager.WorkOrderService.CreateLastNumberWorkOrder();
            if (res.Success && res is ApiOkResponse<int?> result)
            {
                num = result.Result ?? 0;
                Number = AddDynamicLeadingZeros(num);
            }
            else if (res is ApiBadRequestResponse badRequestResponse)
            {
                Number = $"000000";
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
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
        private async Task CloseAsync()
        {
            if(CloseAction != null)
                await CloseAction() ;
            
        }

        private void BrowseToAssigned()
        {
            UserTeamStore = new UserTeamStore();
            UserTeamStore.UserTeamSelected += OnSelectedAssigned;
            SelectUserTeamWindow selectUserTeamWindow = new() { DataContext = new SelectUserTeamWindowViewModel(UserTeamStore) };
            selectUserTeamWindow.ShowDialog();
        }

        private void OnSelectedAssigned(UserTeamDto? dto)
        {
            if (dto?.User is null)
            {
                SelectedAssignedTo = $"Équipe : {dto?.Team?.Name}";
                TeamGuid = dto?.Team?.Id ?? Guid.Empty;
                browseToAssignedVisible = false;
            }
            else if (dto?.Team is null)
            {
                SelectedAssignedTo = $"Utilisateur : {dto?.User?.UserDto.FullName} ";
                UserGuid = dto?.User?.UserDto?.Id ?? Guid.Empty;
                browseToAssignedVisible = false;

            }
        }

        private void BrowseToEquipment()
        {
            SelectedAssetStore = new AssetStore();
            SelectedAssetStore.AssetCreated += OnSelectedEquipment; 
            SelectEquipmentWindow selectEquipmentWindow = new SelectEquipmentWindow() { DataContext = new SelectEquipmentViewModel(SelectedAssetStore)};
            selectEquipmentWindow.ShowDialog();
        }

        private void OnSelectedEquipment(AssetDto? dto)
        {
            SelectedAsset = dto;
            browseToEquipmentVisible = false;
        }

        private void RemoveImage()
        {
            MyImageSource = null;
            IsHaveImage = false;
            
        }

        public async Task Browse()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";


            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
               // ImagePath = Path.GetFileName(selectedFilePath);
                MyImageSource = new BitmapImage(new Uri(selectedFilePath));
                //AssetImage = File.ReadAllBytes(selectedFilePath);
                IsHaveImage = true;
            }
            await Task.Delay(1);
        }

        private async Task UpdateAsync()
        {
           
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                WorkOrderForCreationDto workOrderForUpdateDto = new
                (Name: Name, Number: num, Description: Description, Priority: OrderWorkPriority, StartDate: StartDate,
                 DueDate: DueDate, Type: Type, Status: WorkOrderDto?.Status, Requester: Requester, CreatedOn: WorkOrderDto.CreatedOn,
                 UserCreatedId: Guid.Parse("B04DD1F2-5FF9-4EA0-B7DE-58F5234D426E"),
                 UserAssignedToId: UserGuid == null ? null : UserGuid,
                 TeamAssignedToId: TeamGuid == null ? null : TeamGuid,
                 AssetId: SelectedAsset?.Id == null ? null : SelectedAsset?.Id);
                var Response = await AppServices.ServiceManager.WorkOrderService.UpdateWorkOrder(WorkOrderDto!.Id, workOrderForUpdateDto,true);
                if (Response.Success && Response is ApiOkResponse<WorkOrderDto> result)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Ordre de travail modifié avec succès"));
                    WorkOrderStore.UpdateWorkOrder(result.Result);
                    CloseCommand.Execute(null);
                }else if (Response is ApiBadRequestResponse badRequestResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
                }
            });

        }
    }
}
