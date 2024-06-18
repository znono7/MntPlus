using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ManageWorkViewModel : BaseViewModel
    {
        protected string? mLastSearchText;
        protected string? mSearchText;
        public string? SearchText
        {
            get => mSearchText;
            set
            {
                // Check value is different
                if (mSearchText == value)
                    return;

                // Update value
                mSearchText = value;

                // If the search text is empty...
                if (string.IsNullOrEmpty(SearchText))
                // Search to restore messages
                Search();
            }
        }
        public bool IsActionPopupOpen { get; set; }
        public ICommand OpenActionPopupOpenCommand { get; set; }
        public DueDateFilterViewModel dueDateFilterViewModel { get; set; }
        public bool IsDateFilterOpen { get; set; }  
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
        public bool AddWorkOrderPopupIsOpen { get; set; }


        public ICommand TaskPopupCommand { get; set; }
        public ICommand ViewOrderWorkCommand { get; set; }
        public ICommand OpenDueDateMenuCommand { get; set; }
        public ICommand SearchCommand { get; set; }


        public ViewTaskViewModel TaskViewModel { get; set; } 
        public ShowWorkOrderViewModel ShowWorkOrderViewModel { get; set; }
        public BaseViewModel AddWorkOrderViewModel { get; set; }

        public WorkOrderStore WorkOrderStore { get; set; }

        public bool DimmableOverlayVisible { get; set; }

        private string _statFilterContent;
        public string StatFilterContent
        {
            get => _statFilterContent;
            set
            {
                if (value == _statFilterContent) return;
                _statFilterContent = value;
                OnPropertyChanged(nameof(StatFilterContent));
            }
        }

        private string _dueDateFilterContent = "N'importe quel jour";
        public string DueDatetFilterContent
        {
            get => _dueDateFilterContent;
            set
            {
                if (value == _dueDateFilterContent) return;
                _dueDateFilterContent = value;
                OnPropertyChanged(nameof(DueDatetFilterContent));
            }
        }

        public bool IsMenuStatOpen { get; set; }
        public ICommand OpenStatFilterCommand { get; set; }

        private bool _approvedChecked { get; set; } 
        public bool ApprovedChecked
        {
            get => _approvedChecked; 
            set
            {
                if(value == _approvedChecked) return;
                _approvedChecked = value;
                UpdateStatFilterContent("Approuvé", _approvedChecked);
                OnPropertyChanged(nameof(ApprovedChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.Approved));
            } 
        }

        private bool _pendingChecked { get; set; } 
        public bool PendingChecked
        {
            get => _pendingChecked; 
            set
            {
                if(value == _pendingChecked) return;
                _pendingChecked = value;
                UpdateStatFilterContent("En attente", _pendingChecked);

              
                OnPropertyChanged(nameof(PendingChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.Pending));
            } 
        }

        private bool _openChecked { get; set; } 
        public bool OpenChecked
        {
            get => _openChecked; 
            set
            {
                if(value == _openChecked) return;
                _openChecked = value;
                UpdateStatFilterContent("Ouvrir", _openChecked);

               
                OnPropertyChanged(nameof(OpenChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.Open));
            } 
        }

        private bool _inServiceChecked { get; set; }
        public bool InServiceChecked
        {
            get => _inServiceChecked; 
            set
            {
                if(value == _inServiceChecked) return;
                _inServiceChecked = value;
                UpdateStatFilterContent("En service", _inServiceChecked);


                OnPropertyChanged(nameof(InServiceChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.InService));
            } 
        }

        private bool _completeChecked { get; set; }
        public bool CompleteChecked
        {
            get => _completeChecked; 
            set
            {
                if(value == _completeChecked) return;
                _completeChecked = value;
                UpdateStatFilterContent("Complet", _completeChecked);

              
                OnPropertyChanged(nameof(CompleteChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.Complete));
            } 
        }

        private bool _nonSpecificChecked { get; set; }
        public bool NonSpecificChecked
        {
            get => _nonSpecificChecked; 
            set
            {
                if(value == _nonSpecificChecked) return;
                _nonSpecificChecked = value;
                UpdateStatFilterContent("Non spécifié", _nonSpecificChecked);

               
                OnPropertyChanged(nameof(NonSpecificChecked));
                //FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders.Where(x => x.WorkOrderDto?.Status == WorkOrderStatus.NonSpecific));
            } 
        }

        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<StatFilterCriteria> StatFilterCriteria { get; set; }
        public ManageWorkViewModel()
        {
            _ = LoadWorkOrders();
           
            AddWorkOrderCommand = new RelayCommand(AddWorkOrder);
             
            TaskPopupCommand = new RelayCommand(TaskPopup);
            ViewOrderWorkCommand = new RelayParameterizedCommand(async (dto) => await SetWorkTask(dto));
            //TaskViewModel = new ViewTaskViewModel();
            WorkOrderStore = new WorkOrderStore();
            WorkOrderStore.WorkOrderCreated += WorkOrderStore_WorkOrderCreated;
            WorkOrderStore.WorkOrderUpdated += WorkOrderStore_WorkOrderUpdated;
            WorkOrderStore.WorkOrderDeleted += WorkOrderStore_WorkOrderDeleted;

            OpenStatFilterCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);
            ApprovedChecked = true;
            PendingChecked = true;
            OpenChecked = true;
            dueDateFilterViewModel = new DueDateFilterViewModel();
            OpenDueDateMenuCommand = new RelayCommand(() => IsDateFilterOpen = !IsDateFilterOpen);
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            RemoveCommand = new RelayCommand(async () => await Remove() );
            SearchCommand = new RelayCommand(Search);
            StatFilterCriteria = new ObservableCollection<StatFilterCriteria>();


        }
        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || WorkOrders is null || WorkOrders.Count <= 0)
            {
                // Make filtered list the same
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders ?? Enumerable.Empty<WorkOrderItemsViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(
                WorkOrders.Where(item => item.WorkOrderName is not null && item.WorkOrderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

        private void WorkOrderStore_WorkOrderDeleted(WorkOrderDto? dto)
        {
            var workOrder = WorkOrders.FirstOrDefault(x => x.WorkOrderDto?.Id == dto?.Id);
            //get the index of the work order
            var index = WorkOrders.IndexOf(workOrder!);
            //update the work order
            WorkOrders.RemoveAt(index);
            FilterWorkOrders?.RemoveAt(index);
        }

        private async Task Remove()
        {
            List<WorkOrderItemsViewModel> itemsToRemove = new();
            if (FilterWorkOrders is not null && FilterWorkOrders.Count > 0)
            {
                foreach (var item in FilterWorkOrders)
                {
                    if (item.IsSelected)
                    {
                        itemsToRemove.Add(item);

                    }
                }
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun OT sélectionné"));
                return;
            }
            if (!itemsToRemove.Any())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun Ot sélectionné"));
                return;
            }
            var Dialog = new ConfirmationWindow("Supprimé OT");
            Dialog.ShowDialog();
            if (Dialog.Confirmed)
            {
                foreach (var item in itemsToRemove)
                {
                    await RemoveWorkOrder(item);
                }
            }

        }
        private async Task RemoveWorkOrder(WorkOrderItemsViewModel workOrder)
        {
            var response = await AppServices.ServiceManager.WorkOrderService.DeleteWorkOrder(workOrder.WorkOrderDto!.Id, false);
            if (response is ApiOkResponse<AssetDto> && response.Success)
            {
               
                WorkOrders?.Remove(workOrder);
                FilterWorkOrders?.Remove(workOrder);


            }

        }

        private void UpdateStatFilterContent(string status, bool add)
        {
            var statuses = _statFilterContent?.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>();

            if (add)
            {
                if (!statuses.Contains(status))
                {
                    statuses.Add(status);
                    switch (status)
                    {
                        case "Approuvé":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Approved, "Approuvé"));
                            break;
                        case "En attente":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Waiting, "En attente"));
                            break;
                        case "Ouvrir":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Open, "Ouvrir"));
                            break;
                        case "En service":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.InProgress, "En service"));
                            break;
                        case "Complet":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Completed, "Complet"));
                            break;
                        case "Non spécifié":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.NonSpecific, "Non spécifié"));
                            break;
                    }
                    ApplyFilters();
                }
            }
            else
            {
                statuses.Remove(status);
                switch (status)
                {
                    case "Approuvé":
                        var filter = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.Approved);
                        if (filter != null)
                            RemveFilterCriteria(filter);
                        break;
                    case "En attente":
                        var filter1 = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.Waiting);
                        if (filter1 != null)
                            RemveFilterCriteria(filter1);
                        break;
                    case "Ouvrir":
                        var filter2 = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.Open);
                        if (filter2 != null)
                            RemveFilterCriteria(filter2);
                        break;
                    case "En service":
                        var filter3 = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.InProgress);
                        if (filter3 != null)
                            RemveFilterCriteria(filter3);
                        break;
                    case "Complet":
                        var filter4 = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.Completed);
                        if (filter4 != null)
                            RemveFilterCriteria(filter4);
                        break;
                    case "Non spécifié":
                        var filter5 = StatFilterCriteria.FirstOrDefault(x => x.StatFilterType == StatFilterType.NonSpecific);
                        if (filter5 != null)
                            RemveFilterCriteria(filter5);
                        break;
                }
            }

            StatFilterContent = string.Join(",", statuses);
        }

        private void RemveFilterCriteria(StatFilterCriteria filterToRemove)
        {
            StatFilterCriteria.Remove(filterToRemove);
            if(StatFilterCriteria.Count == 0)
            {
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders ?? Enumerable.Empty<WorkOrderItemsViewModel>());
            }
            else
            {
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            if(WorkOrders is null || WorkOrders.Count == 0) return;

            var items = WorkOrders.ToList();

            foreach (var filter in StatFilterCriteria)
            {
                items = ApplyFilter(items, filter);
            }

            FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(items);
        }

        private List<WorkOrderItemsViewModel> ApplyFilter(List<WorkOrderItemsViewModel> items, StatFilterCriteria filter)
        {
            return filter.StatFilterType switch
            {
                StatFilterType.Approved => items.Where(x => x.WorkOrderDto?.Status == "Approuvé").ToList(),
                StatFilterType.Waiting => items.Where(x => x.WorkOrderDto?.Status == "En attente").ToList(),
                StatFilterType.Open => items.Where(x => x.WorkOrderDto?.Status == "Ouvrir").ToList(),
                StatFilterType.InProgress => items.Where(x => x.WorkOrderDto?.Status == "En service").ToList(),
                StatFilterType.Completed => items.Where(x => x.WorkOrderDto?.Status == "Complet").ToList(),
                StatFilterType.NonSpecific => items.Where(x => x.WorkOrderDto?.Status == "Non spécifique").ToList(),
                _ => items
            };
        }

        private void WorkOrderStore_WorkOrderUpdated(WorkOrderDto? dto)
        {
            var workOrder = WorkOrders.FirstOrDefault(x => x.WorkOrderDto?.Id == dto?.Id);
            //get the index of the work order
            var index = WorkOrders.IndexOf(workOrder!);
            //update the work order 
            WorkOrders[index] = new WorkOrderItemsViewModel(dto);
            FilterWorkOrders![index] = new WorkOrderItemsViewModel(dto);
            
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
            if(dto is null) return;
            WorkOrderDtos ??= new ObservableCollection<WorkOrderDto>();
            WorkOrders ??= new ObservableCollection<WorkOrderItemsViewModel>();
            WorkOrderDtos.Add(dto); 
            WorkOrders.Add(new WorkOrderItemsViewModel(dto));
            FilterWorkOrders ??= new ObservableCollection<WorkOrderItemsViewModel>();
            FilterWorkOrders.Add(new WorkOrderItemsViewModel(dto));


        }

        private void TaskPopup()
        {
            TaskPopupIsOpen = !TaskPopupIsOpen;

        }

        private void AddWorkOrder() 
        {
            AddWorkOrderViewModel = new AddWorkOrderViewModel(WorkOrderStore)
            {
                CloseAction = async () => { AddWorkOrderPopupIsOpen = false; DimmableOverlayVisible = false; await Task.Delay(1); }
            };
            AddWorkOrderPopupIsOpen = true;
            DimmableOverlayVisible = true;
           
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
            ShowWorkOrderViewModel = new ShowWorkOrderViewModel(MyDto.WorkOrderDto,WorkOrderStore); 
            ShowWorkOrderViewModel.CloseAction = CloseTaskPopup;
            ShowWorkOrderViewModel.EditAction = EditWorkOrder;
            TaskPopupIsOpen = true;
            DimmableOverlayVisible = true;
            await Task.Delay(1);
        }

        private async Task EditWorkOrder(WorkOrderDto dto)
        {
            AddWorkOrderViewModel = new AddWorkOrderViewModel(WorkOrderStore , dto)
            {
                CloseAction = async () => { AddWorkOrderPopupIsOpen = false; DimmableOverlayVisible = false; await Task.Delay(1); }
            };
            TaskPopupIsOpen = false;
            AddWorkOrderPopupIsOpen = true;
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
 