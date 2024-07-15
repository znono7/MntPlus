using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ManageWorkViewModel : BaseViewModel
    {
        private string _priorityFilterContent ;
        public string PriorityFilterContent
        {
            get => _priorityFilterContent;
            set
            {
                if (value == _priorityFilterContent) return;
                _priorityFilterContent = value;
                OnPropertyChanged(nameof(PriorityFilterContent));
            }
        } 
        public bool IsPriorityFilterOpen { get; set; }
        public ICommand OpenPriorityFilterCommand { get; set; }
         private bool _lowPriorityChecked { get; set; }
        public bool LowPriorityChecked
        {
            get => _lowPriorityChecked; 
            set
            {
                if(value == _lowPriorityChecked) return;
                _lowPriorityChecked = value;
                UpdatePriorityFilterContent("Basse priorité", _lowPriorityChecked);
                OnPropertyChanged(nameof(LowPriorityChecked));
            } 
        }
         
        private bool _mediumPriorityChecked { get; set; }
        public bool MediumPriorityChecked
        {
            get => _mediumPriorityChecked; 
            set
            {
                if(value == _mediumPriorityChecked) return;
                _mediumPriorityChecked = value;
                UpdatePriorityFilterContent("Moyenne priorité", _mediumPriorityChecked);
                OnPropertyChanged(nameof(MediumPriorityChecked));
            } 
        }
        private bool _highPriorityChecked { get; set; }
        public bool HighPriorityChecked
        {
            get => _highPriorityChecked; 
            set
            {
                if(value == _highPriorityChecked) return;
                _highPriorityChecked = value;
                UpdatePriorityFilterContent("Haute priorité", _highPriorityChecked);
                OnPropertyChanged(nameof(HighPriorityChecked));
            } 
        }
        private bool _noPriorityChecked { get; set; }
        public bool NoPriorityChecked
        {
            get => _noPriorityChecked; 
            set
            {
                if(value == _noPriorityChecked) return;
                _noPriorityChecked = value;
                UpdatePriorityFilterContent("Aucune priorité", _noPriorityChecked);
                OnPropertyChanged(nameof(NoPriorityChecked));
            } 
        }


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

        private string _statFilterContent ;
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
            } 
        }

        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<StatFilterCriteria> StatFilterCriteria { get; set; }
        public ObservableCollection<PriorityFilterCriteria> PriorityFilterCriterias { get; set; }

       
        public bool FilterByStatus { get; set; }
        public bool FilterByPriority { get; set; }
        public ManageWorkViewModel()
        {
            StatFilterCriteria = new ObservableCollection<StatFilterCriteria>();
            PriorityFilterCriterias = new ObservableCollection<PriorityFilterCriteria>();

            _ = LoadWorkOrders();
           
            AddWorkOrderCommand = new RelayCommand(AddWorkOrder);
             
            TaskPopupCommand = new RelayCommand(TaskPopup);
            ViewOrderWorkCommand = new RelayParameterizedCommand(async (dto) => await SetWorkTask(dto));
            WorkOrderStore = new WorkOrderStore();
            WorkOrderStore.WorkOrderCreated += WorkOrderStore_WorkOrderCreated;
            WorkOrderStore.WorkOrderUpdated += WorkOrderStore_WorkOrderUpdated;
            WorkOrderStore.WorkOrderDeleted += WorkOrderStore_WorkOrderDeleted;

            OpenStatFilterCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);

            dueDateFilterViewModel = new DueDateFilterViewModel();
            OpenDueDateMenuCommand = new RelayCommand(() => IsDateFilterOpen = !IsDateFilterOpen);
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            OpenPriorityFilterCommand = new RelayCommand(() => IsPriorityFilterOpen = !IsPriorityFilterOpen);
            RemoveCommand = new RelayCommand(async () => await Remove() );
            SearchCommand = new RelayCommand(Search);


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
            var Dialog = new ConfirmationWindow("Supprimer les OT sélectionnés", "Êtes-vous sûr de vouloir supprimer les OT sélectionnés?");
            Dialog.ShowDialog();
           
            if (Dialog.Confirmed)
            {
                //select Guid from the list of items to remove
                var ids = itemsToRemove.Select(x => x.WorkOrderDto!.Id).ToList();
                var response = await AppServices.ServiceManager.WorkOrderService.BulkDeleteWorkOrder(ids, true);
                if (response is ApiOkResponse<IEnumerable<WorkOrderDto>> && response.Success)
                {
                    foreach (var item in itemsToRemove)
                    {
                        WorkOrders.Remove(item);
                        FilterWorkOrders?.Remove(item);
                        WorkOrderDtos.Remove(item.WorkOrderDto!);
                    }
                }else if(response is ApiBadRequestResponse badRequest)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression des OT"));
                }

               
            }

        }

        #region Filters
        private void UpdatePriorityFilterContent(string v, bool lowPriorityChecked)
        {

            var priorities = _priorityFilterContent?.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>();

            if (lowPriorityChecked)
            {
                FilterByPriority = true;
                if (!priorities.Contains(v))
                {
                    priorities.Add(v);
                    PriorityFilterContent = string.Join(",", priorities);

                    switch (v)
                    {
                        case "Basse priorité":
                            PriorityFilterCriterias.Add(new PriorityFilterCriteria(PriorityFilterType.LowPriority, "Basse priorité"));
                            break;
                        case "Moyenne priorité":
                            PriorityFilterCriterias.Add(new PriorityFilterCriteria(PriorityFilterType.MediumPriority, "Moyenne priorité"));
                            break;
                        case "Haute priorité":
                            PriorityFilterCriterias.Add(new PriorityFilterCriteria(PriorityFilterType.HighPriority, "Haute priorité"));
                            break;
                        case "Aucune priorité":
                            PriorityFilterCriterias.Add(new PriorityFilterCriteria(PriorityFilterType.NoPriority, "Aucune priorité"));
                            break;
                    }
                    ApplyFilters(FilterByStatus, FilterByPriority);
                }
            }
            else
            {
                priorities.Remove(v);
                if (priorities.Count == 0)
                {
                    FilterByPriority = false;
                }
                PriorityFilterContent = string.Join(",", priorities);

                switch (v)
                {
                    case "Basse priorité":
                        var filter = PriorityFilterCriterias.FirstOrDefault(x => x.PriorityFilterType == PriorityFilterType.LowPriority);
                        if (filter != null)
                            RemveFilterCriteria(filter);
                        break;
                    case "Moyenne priorité":
                        var filter1 = PriorityFilterCriterias.FirstOrDefault(x => x.PriorityFilterType == PriorityFilterType.MediumPriority);
                        if (filter1 != null)
                            RemveFilterCriteria(filter1);
                        break;
                    case "Haute priorité":
                        var filter2 = PriorityFilterCriterias.FirstOrDefault(x => x.PriorityFilterType == PriorityFilterType.HighPriority);
                        if (filter2 != null)
                            RemveFilterCriteria(filter2);
                        break;
                    case "Aucune priorité":
                        var filter3 = PriorityFilterCriterias.FirstOrDefault(x => x.PriorityFilterType == PriorityFilterType.NoPriority);
                        if (filter3 != null)
                            RemveFilterCriteria(filter3);
                        break;
                }
            }

        }

        private void UpdateStatFilterContent(string status, bool add)
        {

            List<string> statuses;
            statuses = _statFilterContent?.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToList() ?? new List<string>();
            if (add)
            {
                FilterByStatus = true;
                if (!statuses.Contains(status))
                {
                    statuses.Add(status);
                    _statFilterContent = string.Join(",", statuses);

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
                    ApplyFilters(FilterByStatus, FilterByPriority);
                }
            }
            else
            {
                statuses.Remove(status);
                if (statuses.Count == 0)
                {
                    FilterByStatus = false;
                }
                _statFilterContent = string.Join(",", statuses);

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



        }
        private void RemveFilterCriteria(PriorityFilterCriteria filterToRemove)
        {
            PriorityFilterCriterias.Remove(filterToRemove);
            if (PriorityFilterCriterias.Count == 0 && StatFilterCriteria.Count == 0)
            {
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders ?? Enumerable.Empty<WorkOrderItemsViewModel>());
            }
            else
            {
                ApplyFilters(FilterByStatus, FilterByPriority);
            }
        }
        private void RemveFilterCriteria(StatFilterCriteria filterToRemove)
        {
            StatFilterCriteria.Remove(filterToRemove);
            if (PriorityFilterCriterias.Count == 0 && StatFilterCriteria.Count == 0)
            {
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders ?? Enumerable.Empty<WorkOrderItemsViewModel>());
            }
            else
            {
                ApplyFilters(FilterByStatus, FilterByPriority);
            }
        }

        private void ApplyFilters(bool filterByStatus, bool filterByPriority)
        {
            if (WorkOrders is null || WorkOrders.Count == 0) return;
            if (!filterByStatus && !filterByPriority)
            {
                FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(WorkOrders);
                return;
            }
            var items = WorkOrders.ToList();

            if (filterByPriority)
            {
                // Apply Priority filters first with OR logic
                items = ApplyPriorityFiltersOr(items, PriorityFilterCriterias);
            }

            if (filterByStatus)
            {
                // Apply Status filters next with OR logic
                items = ApplyFiltersOr(items, StatFilterCriteria);
            }

            FilterWorkOrders = new ObservableCollection<WorkOrderItemsViewModel>(items);
        }
        private List<WorkOrderItemsViewModel> ApplyPriorityFiltersOr(List<WorkOrderItemsViewModel> items, IEnumerable<PriorityFilterCriteria> priorityFilters)
        {
            var filteredItems = new List<WorkOrderItemsViewModel>();

            foreach (var filter in priorityFilters)
            {
                filteredItems.AddRange(ApplyPriorityFilter(items, filter));
            }

            return filteredItems.Distinct().ToList();
        }
        private List<WorkOrderItemsViewModel> ApplyPriorityFilter(List<WorkOrderItemsViewModel> items, PriorityFilterCriteria filter)
        {
            return filter.PriorityFilterType switch
            {
                PriorityFilterType.HighPriority => items.Where(x => x.WorkOrderDto?.Priority == "1").ToList(),
                PriorityFilterType.MediumPriority => items.Where(x => x.WorkOrderDto?.Priority == "2").ToList(),
                PriorityFilterType.LowPriority => items.Where(x => x.WorkOrderDto?.Priority == "3").ToList(),
                PriorityFilterType.NoPriority => items.Where(x => x.WorkOrderDto?.Priority == "Aucune").ToList(),
                _ => items
            };
        }
        private List<WorkOrderItemsViewModel> ApplyFiltersOr(List<WorkOrderItemsViewModel> items, IEnumerable<StatFilterCriteria> filters)
        {
            var filteredItems = new List<WorkOrderItemsViewModel>();

            foreach (var filter in filters)
            {
                filteredItems.AddRange(ApplyFilter(items, filter));
            }

            // Remove duplicates
            filteredItems = filteredItems.Distinct().ToList();

            return filteredItems;
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

        #endregion
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
            WorkOrders.Add(new WorkOrderItemsViewModel(dto) { ViewOrderWork = SetWorkTask} );
            FilterWorkOrders ??= new ObservableCollection<WorkOrderItemsViewModel>();
            FilterWorkOrders.Add(new WorkOrderItemsViewModel(dto) { ViewOrderWork = SetWorkTask });


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
 