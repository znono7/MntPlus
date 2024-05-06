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
   public class ManageWorkViewModel : BaseViewModel
    {

        public ObservableCollection<WorkOrderDto> WorkOrderDtos { get; set;} 

        private ObservableCollection<WorkOrderItemsViewModel> _workOrders { get; set; }
        public ObservableCollection<WorkOrderItemsViewModel>? FilterWorkOrders { get; set; }
        public ObservableCollection<WorkOrderItemsViewModel> WorkOrders
        { 
            get => _workOrders; 
            set 
            {
                if(value == _workOrders) return;
                _workOrders = value;
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(_workOrders);
                OnPropertyChanged(nameof(WorkOrders)); 

            } 
        }
        public ICommand AddWorkOrderCommand { get; set; }

        public bool TaskPopupIsOpen { get; set; }

        public ICommand TaskPopupCommand { get; set; }
        public ICommand ViewOrderWorkCommand { get; set; }

        public ViewTaskViewModel TaskViewModel { get; set; } 

        public WorkOrderStore WorkOrderStore { get; set; }

        public bool DimmableOverlayVisible { get; set; }
        public ManageWorkViewModel()
        {
            LoadWorkOrders().GetAwaiter().GetResult();
           
            AddWorkOrderCommand = new RelayCommand(AddWorkOrder);
             
            TaskPopupCommand = new RelayCommand(TaskPopup);
            ViewOrderWorkCommand = new RelayParameterizedCommand(async (dto) => await SetWorkTask(dto));
            //TaskViewModel = new ViewTaskViewModel();
            WorkOrderStore = new WorkOrderStore();
            WorkOrderStore.WorkOrderCreated += WorkOrderStore_WorkOrderCreated;
        }

        private async Task LoadWorkOrders()
        {
            var workOrders = await AppServices.ServiceManager.WorkOrderService.GetAllWorkOrdersAsync(false);
            if(workOrders is not null && workOrders is ApiOkResponse<IEnumerable<WorkOrderDto>> response)
            {
                WorkOrderDtos = new ObservableCollection<WorkOrderDto>(response.Result!);
                OrganizeWorkOrders();

            }
            else if(workOrders is not null && workOrders is ApiBadRequestResponse badRequest)
            {
                WorkOrderDtos = new ObservableCollection<WorkOrderDto>();
            }else if(workOrders is not null && workOrders is ApiNotFoundResponse notFound)
            {
                WorkOrderDtos = new ObservableCollection<WorkOrderDto>();
            }
        }
        private void WorkOrderStore_WorkOrderCreated(WorkOrderDto? dto)
        {
            WorkOrderDtos.Add(dto);
            WorkOrders.Add(new WorkOrderItemsViewModel(dto));
            FilterWorkOrders.Add(new WorkOrderItemsViewModel(dto));


        }

        private void TaskPopup()
        {
            TaskPopupIsOpen = !TaskPopupIsOpen;

        }

        private void AddWorkOrder() 
        {
           var wind =  new SelectEquipmentWindow { DataContext = new SelectEquipmentViewModel(WorkOrderStore) };
            wind.ShowDialog();
        }

        public void OrganizeWorkOrders()
        {
            List< WorkOrderItemsViewModel> workOrderItems = new List<WorkOrderItemsViewModel>();
            foreach (var item in WorkOrderDtos)
            {
                workOrderItems.Add(new WorkOrderItemsViewModel(item));
            }
            WorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(workOrderItems);
            foreach (var item in workOrderItems)
            {
                item.ViewOrderWork = SetWorkTask;
            }
        }

        private async Task SetWorkTask(object? dto)
        {
            if(dto is null) return;
            var MyDto = (WorkOrderItemsViewModel)dto;
            TaskViewModel = new ViewTaskViewModel(MyDto.WorkOrderDto);
            TaskViewModel.ClosePopupAction = CloseTaskPopup;
            TaskPopupIsOpen = true;
            DimmableOverlayVisible = true;
            await Task.Delay(1);
        }

        private async Task CloseTaskPopup()
        {
            TaskPopupIsOpen = false;
            DimmableOverlayVisible = false;
            await Task.Delay(1);
        }
    }
}
 