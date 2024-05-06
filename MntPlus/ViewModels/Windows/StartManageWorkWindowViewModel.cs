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
    public class StartManageWorkWindowViewModel : BaseViewModel
    {
        public string OrderWorkName { get; set; }
        public string? OrderWorkInstructions { get; set; } 

        private string _orderWorkPriority = "3";
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
                }
            }
        }

        private DateTime _dueDate = DateTime.Today;
        public DateTime DueDate { get => _dueDate; set => _dueDate = value.Date; }
        public string? ShortDueDate => DueDate.ToShortDateString();

        public string Type { get; set; } = "Prévu";
        public bool IsMenuPrioprityOpen { get; set; }
        public bool IsMenuDueDateOpen { get; set; }

        public ICommand OpenMenuPriorityCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }

        public ICommand HighPriorityCommand { get; set; }
        public ICommand MediumPriorityCommand { get; set; }
        public ICommand LowPriorityCommand { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelOrderWorkCommand { get; set; }


        public string ForgroundColor { get; set; } = "429b1f";
        public AssetDto Equipment { get; }

        public string? EquipmentName => Equipment.Name;

        public WorkOrderStore? WorkOrderStore { get; set; } 

        public string? Name { get; set; }
        public string? Instructions { get; set; }

        public bool SaveIsRunning { get; set; }

        public StartManageWorkWindowViewModel(AssetDto equipment , WorkOrderStore? workOrderStore) 
        {
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "1");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "2");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "3");
            SaveCommand = new RelayParameterizedCommand(async(parameter) => await Save(parameter));
            CancelOrderWorkCommand = new RelayParameterizedCommand(async(parameter) => await CancelOrderWork(parameter));
            Equipment = equipment;
            WorkOrderStore = workOrderStore;
        }

        private async Task CancelOrderWork(object? parameter)
        {
            var window = (System.Windows.Window)parameter!;
            window.Close();
            await Task.Delay(1);

        }

        private async Task Save(object? parameter)
        {
            if(!CanSave())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez remplir tous les champs"));
                return;
            }
            var window = (System.Windows.Window)parameter!;
            await RunCommandAsync(() => SaveIsRunning, async () =>
            {
                var workOrder = await AppServices.ServiceManager.WorkOrderService.CreateWorkOrder(new WorkOrderForCreationDto
               (
               Name, Instructions, OrderWorkPriority, DueDate, Type, "En attente", null, null, Equipment.Id
               ));
                if (workOrder is not null && workOrder is ApiOkResponse<WorkOrderDto> okRespons)
                {
                    await Task.Delay(1000);
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Ordre de travail créé avec succès"));
                    WorkOrderStore?.CreateWorkOrder(okRespons.Result);
                    window.Close();

                }
                else if (workOrder is not null && workOrder is ApiBadRequestResponse badRequest)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequest.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Une erreur s'est produite lors de la création de l'ordre de travail"));
                }
            }
            );
           
              
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);//&& !string.IsNullOrEmpty(Instructions) ;
        }
    }
}
