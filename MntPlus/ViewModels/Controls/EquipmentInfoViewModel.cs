using Entities;
using Entities.Responses.Equipment;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {
        public List<EquipmentTypeDto>? EquipmentTypes { get; set; }
        public EquipmentTypeDto? SelectedEquipmentType { get; set; }

        public ObservableCollection<EquipmentOrganizationDto>? Organizations { get; set; }
        public EquipmentOrganizationDto? SelectedOrganization { get; set; }

        public ObservableCollection<EquipmentDepartmentDto>? Departments { get; set; }
        public EquipmentDepartmentDto? SelectedDepartment { get; set; }

        public ObservableCollection<EquipmentClassDto>? EquipmentClasses { get; set; }
        public EquipmentClassDto? SelectedEquipmentClass { get; set; }

        public List<EquipmentStatusDto>? EquipmentStatuses { get; set; }
        public EquipmentStatusDto? SelectedEquipmentStatus { get; set; }

        public ObservableCollection<EquipmentAssignedToDto>? Assignees { get; set; }
        public EquipmentAssignedToDto? SelectedAssignee { get; set; }

        public string EquipmentName { get; set; }
        public string? EquipmentDescription { get; set; }
        public string? EquipmentSite { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentSerialNumber { get; set; }
        public string? EquipmentModel { get; set; }
        public double? EquipmentCost { get; set; }
        public DateTime? EquipmentCommissionDate { get; set; }


        public string? AssigneeNameAdded { get; set; }
        public string? DepartementAdded { get; set; }
        public string? ClassAdded { get; set; }
        public string? OrganizationAdded { get; set; }
        public string? TypeAdded { get; set; }
        public string? StatusAdded { get; set; }
        public EquipmentDto Equipment { get; set; }
        public EquipmentStore EquipmentStore { get; }
        public string? EquipmentNameImage { get; set; }
        public byte[]? EquipmentImage { get; set; }

        public bool AssignPopup { get; set; }
        public bool DatePopup { get; set; }
        public bool OrganisationPopup { get; set; }
        public bool DepartmentPopup { get; set; }
        public bool ClassPopup { get; set; }
        public bool SetAssignPopup { get; set; }
        public bool DimmableOverlayVisible => AssignPopup || DatePopup || OrganisationPopup || DepartmentPopup || ClassPopup || SetAssignPopup;
        public ICommand AddAssigneeCommand { get; set; }
        public ICommand CloseAddAssigneeCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand SaveNewAssigneeCommand { get; set; }
        public ICommand CommissionDateCommand { get; set; }
        public ICommand CloseCommissionDateCommand { get; set; }
        public ICommand ShowOrganizationCommand { get; set; }
        public ICommand CloseOrganizationCommand { get; set; }
        public ICommand SaveNewOrganizationCommand { get; set; }
        public ICommand AddDepartementCommand { get; set; }
        public ICommand CloseDepartementCommand { get; set; }
        public ICommand SaveNewDepartementCommand { get; set; }
        public ICommand AddClassCommand { get; set; }
        public ICommand CloseClassCommand { get; set; }
        public ICommand SaveNewClassCommand { get; set; }
        public ICommand PopupClickawayCommand { get; set; }

        public ICommand OpenSettingAnAssignor { get; set; }
        public ICommand CloseSettingAnAssignor { get; set; }
        public ICommand SetAnAssignorCommand { get; set; }
        public EquipmentInfoViewModel(EquipmentDto equipment , EquipmentStore equipmentStore)
        {
            LoadDataAsync().GetAwaiter().GetResult();
            Equipment = equipment;
            EquipmentStore = equipmentStore;
            MapEquipmentDtoToEquipmentInfoViewModel();
            AddAssigneeCommand = new RelayCommand(() => AssignPopup = true);
            CloseAddAssigneeCommand = new RelayCommand(() => AssignPopup = false) ;
            CommissionDateCommand = new RelayCommand(() => DatePopup = !DatePopup);
            CloseCommissionDateCommand = new RelayCommand(() => DatePopup = false);
            ShowOrganizationCommand = new RelayCommand(() => OrganisationPopup = true);
            CloseOrganizationCommand = new RelayCommand(() => OrganisationPopup = false);
            AddDepartementCommand = new RelayCommand(() => DepartmentPopup = true);
            CloseDepartementCommand = new RelayCommand(() => DepartmentPopup = false);
            AddClassCommand = new RelayCommand(() => ClassPopup = true);
            CloseClassCommand = new RelayCommand(() => ClassPopup = false);
            SaveNewClassCommand = new RelayCommand(async () => await SaveNewClass());
            SaveNewDepartementCommand = new RelayCommand(async () => await SaveNewDepartement());
            UpdateCommand = new RelayCommand (async() => await UpdateEquipment());
            SaveNewAssigneeCommand = new RelayCommand(async () => await SaveNewAssignee());
            SaveNewOrganizationCommand = new RelayCommand(async () => await SaveNewOrganization());
            OpenSettingAnAssignor = new RelayCommand(() => SetAssignPopup = true);
            CloseSettingAnAssignor = new RelayCommand(() => SetAssignPopup = false);
            PopupClickawayCommand = new RelayCommand(PopupStateChange);
            SetAnAssignorCommand = new RelayCommand(SetAnAssignor);
        }

        private void SetAnAssignor()
        {
            if(SelectedAssignee is null)
            {
                return;
            }
            SetAssignPopup = false;
        }

        private void PopupStateChange()
        {
            AssignPopup = false;
            DatePopup = false;
            OrganisationPopup = false;
            DepartmentPopup = false;
            ClassPopup = false;
            SetAssignPopup = false;
        }
        private async Task LoadDataAsync()
        {
            await Task.WhenAll(
                LoadAssignee(),
                LoadDepartments(),
                LoadClasses(),
                LoadOrganizations(),
                LoadEquipmentTypes(),
                LoadEquipmentStatuses()
            );
        }
        private async Task LoadAssignee() 
        {          
            var assignees = await AppServices.ServiceManager.AssignorService.GetAllAssignorsAsync(false);
            if(assignees is not null && assignees is ApiOkResponse<IEnumerable<EquipmentAssignedToDto>> response)
            {
                Assignees = new ObservableCollection<EquipmentAssignedToDto>(response.Result);
            }else if(assignees is not null && assignees is AssignorListNotFoundResponse responseR)
            {
                Assignees = new ObservableCollection<EquipmentAssignedToDto>();
            }else
            {
                Assignees = new ObservableCollection<EquipmentAssignedToDto>();
            }
        }
        private async Task LoadDepartments()
        {
            var departments = await AppServices.ServiceManager.EquipmentDepartmentService.GetAllEquipmentDepartmentsAsync(false);
            if(departments is not null && departments is ApiOkResponse<IEnumerable<EquipmentDepartmentDto>> response)
            {
                Departments = new ObservableCollection<EquipmentDepartmentDto>(response.Result);
            }else if(departments is not null && departments is AssignorListNotFoundResponse responseR)
            {
                Departments = new ObservableCollection<EquipmentDepartmentDto>();
            }else
            {
                Departments = new ObservableCollection<EquipmentDepartmentDto>();
            }
        }
        private async Task LoadClasses()
        {
            var classes = await AppServices.ServiceManager.EquipmentClassService.GetAllEquipmentClassesAsync(false);
            if(classes is not null && classes is ApiOkResponse<IEnumerable<EquipmentClassDto>> response)
            {
                EquipmentClasses = new ObservableCollection<EquipmentClassDto>(response.Result);
            }else if(classes is not null && classes is AssignorListNotFoundResponse responseR)
            {
                EquipmentClasses = new ObservableCollection<EquipmentClassDto>();
            }else
            {
                EquipmentClasses = new ObservableCollection<EquipmentClassDto>();
            }
        }
        private async Task LoadOrganizations()
        {
            var organizations = await AppServices.ServiceManager.EquipmentOrganizationService.GetEquipmentOrganizationsAsync(false);
            if(organizations is not null && organizations is ApiOkResponse<IEnumerable<EquipmentOrganizationDto>> response)
            {
                Organizations = new ObservableCollection<EquipmentOrganizationDto>(response.Result);
            }else if(organizations is not null && organizations is AssignorListNotFoundResponse responseR)
            {
                Organizations = new ObservableCollection<EquipmentOrganizationDto>();
            }else
            {
                Organizations = new ObservableCollection<EquipmentOrganizationDto>();
            }
        }
        private async Task LoadEquipmentTypes()
        {
            var equipmentTypes = await AppServices.ServiceManager.EquipmentTypeService.GetAllEquipmentTypesAsync(false);
            if(equipmentTypes is not null && equipmentTypes is ApiOkResponse<IEnumerable<EquipmentTypeDto>> response)
            {
                EquipmentTypes = response.Result.ToList();
            }else if(equipmentTypes is not null && equipmentTypes is AssignorListNotFoundResponse responseR)
            {
                EquipmentTypes = new List<EquipmentTypeDto>();
            }else
            {
                EquipmentTypes = new List<EquipmentTypeDto>();
            }
        }
        private async Task LoadEquipmentStatuses()
        {
            var equipmentStatuses = await AppServices.ServiceManager.EquipmentStatusService.GetAllEquipmentStatusesAsync(false);
            if(equipmentStatuses is not null && equipmentStatuses is ApiOkResponse<IEnumerable<EquipmentStatusDto>> response)
            {
                EquipmentStatuses = response.Result.ToList();
            }else if(equipmentStatuses is not null && equipmentStatuses is AssignorListNotFoundResponse responseR)
            {
                EquipmentStatuses = new List<EquipmentStatusDto>();
            }else
            {
                EquipmentStatuses = new List<EquipmentStatusDto>();
            }
        }

        public async Task SaveNewStatus()
        {
            if(string.IsNullOrEmpty(StatusAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom du statut est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.EquipmentStatusService.CreateEquipmentStatus(new EquipmentStatusDtoCreate(StatusAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le statut n'a pas été créé"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentStatusDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"Le statut a été créé avec succès {responseR.Result.StatusName}"));
                EquipmentStatuses.Add(new EquipmentStatusDto(responseR.Result.Id, responseR.Result.StatusName));
            }

        }
        public async Task SaveNewType()
        {
            if(string.IsNullOrEmpty(TypeAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom du type est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.EquipmentTypeService.CreateEquipmentType(new EquipmentTypeDtoCreate(TypeAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le type n'a pas été créé"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentTypeDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"Le type a été créé avec succès {responseR.Result.EquipmentTypeName}"));
                EquipmentTypes.Add(new EquipmentTypeDto(responseR.Result.Id, responseR.Result.EquipmentTypeName));
            }

        }

        private async Task SaveNewClass()
        {
            if(string.IsNullOrEmpty(ClassAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom de la classe est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.EquipmentClassService.CreateEquipmentClass(new EquipmentClassForCreationDto(ClassAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La classe n'a pas été créée"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentClassDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"La classe a été créée avec succès {responseR.Result.ClassName}"));
                EquipmentClasses.Add(new EquipmentClassDto ( responseR.Result.Id, responseR.Result.ClassName ));
               
            }
        }

        private async Task SaveNewDepartement()
        {
            if(string.IsNullOrEmpty(DepartementAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom du département est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.EquipmentDepartmentService.CreateEquipmentDepartment(new EquipmentDepartmentCreateDto(DepartementAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le département n'a pas été créé"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentDepartmentDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"Le département a été créé avec succès {responseR.Result.DepartmentName}"));
                Departments.Add(new EquipmentDepartmentDto(responseR.Result.Id, responseR.Result.DepartmentName));
               
            }
            
        }

        private async Task SaveNewOrganization()
        {
            if(string.IsNullOrEmpty(OrganizationAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom de l'organisation est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.EquipmentOrganizationService.CreateEquipmentOrganizationAsync(new EquipmentOrganizationCreateDto(OrganizationAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'organisation n'a pas été créée"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentOrganizationDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"L'organisation a été créée avec succès {responseR.Result.OrganizationName}"));
                Organizations.Add(new EquipmentOrganizationDto(responseR.Result.Id, responseR.Result.OrganizationName));
               
            }
            
        }

        private async Task SaveNewAssignee()
        {
            if(string.IsNullOrEmpty(AssigneeNameAdded))
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le nom de l'assigné est obligatoire."));
                return;
            }
            var Result = await AppServices.ServiceManager.AssignorService.CreateAssignor(new AssignedToForCreationDto(AssigneeNameAdded));
            if(Result is not null && Result is AssignorCreateErrorResponse response)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "L'assigné n'a pas été créé"));
            }else if (Result is not null && Result is ApiOkResponse<EquipmentAssignedToDto> responseR)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"L'assigné a été créé avec succès"));
                Assignees.Add(new EquipmentAssignedToDto(responseR.Result.Id,responseR.Result.AssignedToName));
            }
            
        }

        private async Task UpdateEquipment()
        {
            var quipmentForUpdateDto = new EquipmentForUpdateDto
            (
                EquipmentName : EquipmentName,
                                EquipmentType: SelectedEquipmentType is null ? null : SelectedEquipmentType,
                                EquipmentDescription: EquipmentDescription,
                                EquipmentOrganization: SelectedOrganization is null ? null : SelectedOrganization,
                                EquipmentDepartment: SelectedDepartment is null ? null : SelectedDepartment,
                                EquipmentClass: SelectedEquipmentClass is null ? null : SelectedEquipmentClass,
                                EquipmentSite: EquipmentSite,
                                EquipmentStatus: SelectedEquipmentStatus is null ? null : SelectedEquipmentStatus,
                                EquipmentMake: EquipmentMake,
                                EquipmentSerialNumber: EquipmentSerialNumber,

                EquipmentModel: EquipmentModel,
                EquipmentCost: EquipmentCost,
                EquipmentCommissionDate: EquipmentCommissionDate,
                EquipmentAssignedTo : SelectedAssignee is null ? null : SelectedAssignee,
                EquipmentNameImage: EquipmentNameImage,
                EquipmentImage: EquipmentImage

            );
            var Result = await AppServices.ServiceManager.EquipmentService.UpdateEquipmentAsync(Equipment.Id, quipmentForUpdateDto, true);
            if(Result is not null && Result is EquipmentDeleteErrorResponse)
            {
               await IoContainer.NotificationsManager.ShowMessage( new NotificationControlViewModel(NotificationType.Error, "Equipment was not updated"));

            }else if(Result is not null && Result is ApiOkResponse<Equipment> response)
            {
               await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, $"Equipment was updated successfully{response.Result.EquipmentName}"));
               EquipmentStore.UpdateEquipment(MapToEquipmentDTO(response.Result));
            }else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Equipment was not updated"));
            }
        }



        //map properties from EquipmentDto to EquipmentInfoViewModel
        private void MapEquipmentDtoToEquipmentInfoViewModel()
        {
            EquipmentName = Equipment.EquipmentName;
            EquipmentDescription = Equipment.EquipmentDescription;
            EquipmentSite = Equipment.EquipmentSite;
            EquipmentMake = Equipment.EquipmentMake;
            EquipmentSerialNumber = Equipment.EquipmentSerialNumber;
            EquipmentModel = Equipment.EquipmentModel;
            EquipmentCost = Equipment.EquipmentCost;
            EquipmentCommissionDate = Equipment.EquipmentCommissionDate;
            if(Equipment.EquipmentType is not null)
            {
                SelectedEquipmentType = Equipment.EquipmentType;

            }
            if(Equipment.EquipmentOrganization is not null)
            {
                SelectedOrganization = Equipment.EquipmentOrganization;
             

            }
            if(Equipment.EquipmentDepartment is not null)
            {
                SelectedDepartment = Equipment.EquipmentDepartment;
             

            }
            if(Equipment.EquipmentClass is not null)
            {
                SelectedEquipmentClass = Equipment.EquipmentClass;
                

            }
            if(Equipment.EquipmentStatus is not null)
            {
                SelectedEquipmentStatus = Equipment.EquipmentStatus;
            }
            if(Equipment.EquipmentAssignedTo is not null)
            {
                SelectedAssignee = Equipment.EquipmentAssignedTo;
            }
           
        }

        private EquipmentDto? MapToEquipmentDTO(Equipment? equipment)
        {
           
            return new EquipmentDto
            (
                Id: equipment.Id,
                EquipmentParent: equipment.EquipmentParent,
                EquipmentName: equipment.EquipmentName,
                EquipmentType: equipment.EquipmentType is null ? null : new EquipmentTypeDto(Id: equipment.EquipmentType.Id, EquipmentTypeName: equipment.EquipmentType.EquipmentTypeName),
                EquipmentDescription: equipment.EquipmentDescription,
                EquipmentOrganization: equipment.EquipmentOrganization is null ? null : new EquipmentOrganizationDto(Id: equipment.EquipmentOrganization.Id, OrganizationName: equipment.EquipmentOrganization.OrganizationName),
                EquipmentDepartment: equipment.EquipmentDepartment is null ? null : new EquipmentDepartmentDto(Id: equipment.EquipmentDepartment.Id, DepartmentName: equipment.EquipmentDepartment.DepartmentName),
                EquipmentClass: equipment.EquipmentClass is null ? null : new EquipmentClassDto(Id: equipment.EquipmentClass.Id, ClassName: equipment.EquipmentClass.EquipmentClassName),
                EquipmentSite: equipment.EquipmentSite,
                EquipmentStatus: equipment.EquipmentStatus is null ? null : new EquipmentStatusDto(Id: equipment.EquipmentStatus.Id, StatusName: equipment.EquipmentStatus.EquipmentStatusName),
                EquipmentMake: equipment.EquipmentMake,
                EquipmentSerialNumber: equipment.EquipmentSerialNumber,
                EquipmentModel: equipment.EquipmentModel,
                EquipmentCost: equipment.EquipmentCost,
                EquipmentCommissionDate: equipment.EquipmentCommissionDate,
                EquipmentAssignedTo: equipment.EquipmentAssignedTo is null ? null : new EquipmentAssignedToDto(Id: equipment.EquipmentAssignedTo.Id, AssignedToName: equipment.EquipmentAssignedTo.Name),
                EquipmentNameImage: equipment.EquipmentNameImage,
                EquipmentImage: equipment.EquipmentImage
            );
        }

    }
}
