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
    public class RequestsPageViewModel : BaseViewModel
    {
        private string _priorityFilterContent;
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
                if (value == _lowPriorityChecked) return;
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
                if (value == _mediumPriorityChecked) return;
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
                if (value == _highPriorityChecked) return;
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
                if (value == _noPriorityChecked) return;
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
        public BaseViewModel requestControlViewModel { get; set; }
        public bool IsRequestControlVisible { get; set; }
        public bool DimmableOverlayVisible { get; set; }

        public ICommand ShowRequestControlCommand { get; set; }
        public RequestStore RequestStore { get; set; }
        public ICommand SearchCommand { get; set; }

        public bool IsMenuStatOpen { get; set; }
        public ICommand OpenStatFilterCommand { get; set; }

        private bool _approvedChecked { get; set; }
        public bool ApprovedChecked
        {
            get => _approvedChecked;
            set
            {
                if (value == _approvedChecked) return;
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
                if (value == _pendingChecked) return;
                _pendingChecked = value;
                UpdateStatFilterContent("En attente", _pendingChecked);


                OnPropertyChanged(nameof(PendingChecked));
            }
        }
        private bool _diclineChecked { get; set; }
        public bool DiclineChecked
        {
            get => _diclineChecked;
            set
            {
                if (value == _diclineChecked) return;
                _diclineChecked = value;
                UpdateStatFilterContent("Refuser", _diclineChecked);
                OnPropertyChanged(nameof(DiclineChecked));
            }
        }
        public ObservableCollection<RequestDto> RequestDtos { get; set; }
        private ObservableCollection<RequestItemViewModel> requestItemViewModels { get; set; }
        public ObservableCollection<RequestItemViewModel>? FilterRequestItems { get; set; }
        public ObservableCollection<RequestItemViewModel> RequestItems
        {
            get => requestItemViewModels;
            set
            {
                if (value == requestItemViewModels) return;
                requestItemViewModels = value;
                FilterRequestItems = new ObservableCollection<RequestItemViewModel>(requestItemViewModels);
                OnPropertyChanged(nameof(RequestItems));

            }
        }
        public ICommand RemoveCommand { get; set; }

        public ObservableCollection<StatFilterCriteria> StatFilterCriteria { get; set; }
        public ObservableCollection<PriorityFilterCriteria> PriorityFilterCriterias { get; set; }


        public bool FilterByStatus { get; set; }
        public bool FilterByPriority { get; set; }

        public ShowRequestViewModel ShowRequestControl { get; set; }
        public bool OpenRequestItemControl { get; set; }
        public ICommand OpenRequestCommand { get; set; }
        public RequestsPageViewModel()
        {
            IsRequestControlVisible = false;
            DimmableOverlayVisible = false;
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            OpenPriorityFilterCommand = new RelayCommand(() => IsPriorityFilterOpen = !IsPriorityFilterOpen);
            SearchCommand = new RelayCommand(Search);
            OpenStatFilterCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);
            OpenRequestCommand = new RelayParameterizedCommand( async (p) => await OpenRequest(p));
            ShowRequestControlCommand = new RelayCommand(ShowControl);
            _ = LoadRequests();
            RequestStore = new RequestStore();
            RequestStore.RequestRemoved += RemoveRequest;
            RequestStore.RequestUpdated += UpdateRequest;
            RequestStore.RequestCreated += CreateReq;

            RemoveCommand = new RelayCommand(async () => await RemoveRequests());
            PriorityFilterCriterias = new ObservableCollection<PriorityFilterCriteria>();
            StatFilterCriteria = new ObservableCollection<StatFilterCriteria>();

        }

        private async Task RemoveRequests()
        {
            List<RequestItemViewModel> itemsToRemove = new();
            if (FilterRequestItems is not null && FilterRequestItems.Count > 0)
            {
                foreach (var item in FilterRequestItems)
                {
                    if (item.IsSelected)
                    {
                        itemsToRemove.Add(item);

                    }
                }
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun Demande sélectionné"));
                return;
            }
            if (!itemsToRemove.Any())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun Demande sélectionné"));
                return;
            }
            var Dialog = new ConfirmationWindow("Supprimer les demandes", "Voulez-vous vraiment supprimer les demandes sélectionnées?");
            Dialog.ShowDialog();
            if (Dialog.Confirmed == true)
            {
               //get Guids of the selected requests
                var guids =  itemsToRemove
                                .Select(x => x.RequestDto?.Id)
                                .Where(id => id.HasValue)  // Filter out null values
                                .Select(id => id!.Value)    // Convert nullable GUID to non-nullable
                                .ToList();
                var result = await AppServices.ServiceManager.RequestService.BulkDeleteRequest(guids,true);
                if (result is not null && result is ApiOkResponse<string> response)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Demandes supprimées avec succès"));
                    foreach (var item in itemsToRemove)
                    {
                        RequestItems.Remove(item);
                        FilterRequestItems.Remove(item);
                        RequestDtos.Remove(item.RequestDto!);
                    }
                   
                }
                else if (result is not null && result is ApiBadRequestResponse badRequest)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequest.Message));
                }
                else
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression des demandes"));
                }
            }
        }

        private async Task LoadRequests()
        {
            var requests = await AppServices.ServiceManager.RequestService.GetAllRequestsAsync(false);
            if (requests is not null && requests is ApiOkResponse<IEnumerable<RequestDto>> result)
            {
                RequestDtos = new ObservableCollection<RequestDto>(result.Result!);
                OrganizeRequests();
            }else
            {
                RequestItems = new ObservableCollection<RequestItemViewModel>();
            }
        }

        private void OrganizeRequests()
        {
            if (RequestDtos is null || RequestDtos.Count == 0) return;
            RequestItems = new ObservableCollection<RequestItemViewModel>(RequestDtos.Select(x => new RequestItemViewModel(x) ));
        }

        private async Task OpenRequest(object? dto)
        {
            var request = dto as RequestItemViewModel;
           

            ShowRequestControl = new ShowRequestViewModel(request?.RequestDto , RequestStore)
            {
                CloseAction = CloseControl,
                EditAction = EditRequest
            };
            OpenRequestItemControl = true;
            DimmableOverlayVisible = true;
            await Task.Delay(1);
        }

        private void RemoveRequest(RequestDto? dto)
        {
            var request = RequestItems.FirstOrDefault(x => x.RequestDto?.Id == dto?.Id);
            RequestItems.Remove(request!);
            FilterRequestItems?.Remove(request!);
            RequestDtos.Remove(dto!);
        }

        private async Task EditRequest(RequestDto dto)
        {
            requestControlViewModel = new RequestControlViewModel(RequestStore , dto)
            {
                CloseAction = CloseControl,

            };
            OpenRequestItemControl = false;
            IsRequestControlVisible = true;
            DimmableOverlayVisible = true;
            await Task.Delay(1);
        }

        private void UpdateRequest(RequestDto? dto)
        {
            //find the request in the list and update it
            var request = RequestItems.FirstOrDefault(x => x.RequestDto?.Id == dto?.Id);
            //find the index of the request
            var index = RequestItems.IndexOf(request!);
            //update the request
            RequestItems[index] = new RequestItemViewModel(dto);
            //update the filter request
            FilterRequestItems![index] = new RequestItemViewModel(dto);
        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || RequestItems is null || RequestItems.Count <= 0)
            {
                // Make filtered list the same
                FilterRequestItems = new ObservableCollection<RequestItemViewModel>(RequestItems ?? Enumerable.Empty<RequestItemViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterRequestItems = new ObservableCollection<RequestItemViewModel>(
                RequestItems.Where(item => item.WorkOrderName is not null && item.WorkOrderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }
        private void ShowControl()
        {
            requestControlViewModel = new RequestControlViewModel(RequestStore)
            {
                CloseAction = CloseControl,
                
            };
            IsRequestControlVisible = true;
            DimmableOverlayVisible = true;
        }

        private void CreateReq(RequestDto? dto)
        {
            RequestItems ??= new ObservableCollection<RequestItemViewModel>();
            RequestItems.Add(new RequestItemViewModel(dto));
            FilterRequestItems?.Add(new RequestItemViewModel(dto));
            
        }

        private void ShowOverlay()
        {
            IsRequestControlVisible = true;
            DimmableOverlayVisible = true;
        }
        private async Task CloseControl()
        {
            HideOverlay();
            await Task.CompletedTask;
        }
        private void HideOverlay()
        {
            IsRequestControlVisible = false;
            DimmableOverlayVisible = false;
            OpenRequestItemControl = false;
        }

        #region Filters
        private void UpdatePriorityFilterContent(string v, bool lowPriorityChecked)
        {
            PriorityFilterCriterias ??= new ObservableCollection<PriorityFilterCriteria>();
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
            StatFilterCriteria ??= new ObservableCollection<StatFilterCriteria>();
             if (add)
            {
                FilterByStatus = true;
               
                    switch (status)
                    {
                        case "Approuvé":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Approved, "Approuvé"));
                            break;
                        case "En attente":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.Waiting, "En attente"));
                            break;
                        case "Refuser":
                            StatFilterCriteria.Add(new StatFilterCriteria(StatFilterType.NonSpecific, "Refuser"));
                            break;
                    }
                    ApplyFilters(FilterByStatus, FilterByPriority);
                
            }
            else
            {
                if (StatFilterCriteria.Count == 0)
                {
                    FilterByStatus = false;
                }

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
                  
                    case "Refuser":
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
                FilterRequestItems = new ObservableCollection<RequestItemViewModel>(RequestItems ?? Enumerable.Empty<RequestItemViewModel>());
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
                FilterRequestItems = new ObservableCollection<RequestItemViewModel>(RequestItems ?? Enumerable.Empty<RequestItemViewModel>());
            }
            else
            {
                ApplyFilters(FilterByStatus, FilterByPriority);
            }
        }

        private void ApplyFilters(bool filterByStatus, bool filterByPriority)
        {
            if (RequestItems is null || RequestItems.Count == 0) return;
            if (!filterByStatus && !filterByPriority)
            {
                FilterRequestItems = new ObservableCollection<RequestItemViewModel>(RequestItems);
                return;
            }
            var items = RequestItems.ToList();

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

            FilterRequestItems = new ObservableCollection<RequestItemViewModel>(items);
        }
        private List<RequestItemViewModel> ApplyPriorityFiltersOr(List<RequestItemViewModel> items, IEnumerable<PriorityFilterCriteria> priorityFilters)
        {
            var filteredItems = new List<RequestItemViewModel>();

            foreach (var filter in priorityFilters)
            {
                filteredItems.AddRange(ApplyPriorityFilter(items, filter));
            }

            return filteredItems.Distinct().ToList();
        }
        private List<RequestItemViewModel> ApplyPriorityFilter(List<RequestItemViewModel> items, PriorityFilterCriteria filter)
        {
            return filter.PriorityFilterType switch
            {
                PriorityFilterType.HighPriority => items.Where(x => x.RequestDto?.Priority == "1").ToList(),
                PriorityFilterType.MediumPriority => items.Where(x => x.RequestDto?.Priority == "2").ToList(),
                PriorityFilterType.LowPriority => items.Where(x => x.RequestDto?.Priority == "3").ToList(),
                PriorityFilterType.NoPriority => items.Where(x => x.RequestDto?.Priority == "Aucune").ToList(),
                _ => items
            };
        }
        private List<RequestItemViewModel> ApplyFiltersOr(List<RequestItemViewModel> items, IEnumerable<StatFilterCriteria> filters)
        {
            var filteredItems = new List<RequestItemViewModel>();

            foreach (var filter in filters)
            {
                filteredItems.AddRange(ApplyFilter(items, filter));
            }

            // Remove duplicates
            filteredItems = filteredItems.Distinct().ToList();

            return filteredItems;
        }
        private List<RequestItemViewModel> ApplyFilter(List<RequestItemViewModel> items, StatFilterCriteria filter)
        {
            return filter.StatFilterType switch
            {
                StatFilterType.Approved => items.Where(x => x.RequestDto?.Status == "Approuvé").ToList(),
                StatFilterType.Waiting => items.Where(x => x.RequestDto?.Status == "En attente").ToList(),
                StatFilterType.NonSpecific => items.Where(x => x.RequestDto?.Status == "Refuser").ToList(),
                _ => items
            };
        }

        #endregion
    }
}
 