using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace MntPlus.WPF
{
    public class PreventiveMaintenanceViewModel : BaseViewModel
    {
        public ObservableCollection<FilterPmViewModel> Filters { get; set; }
        public ICommand AddFilterCommand { get; set; }
        public ICommand ClearAllFiltersCommand { get; set; }

        public ObservableCollection<string> AvailableFields { get; set; }
        public ObservableCollection<string> AvailableOperators { get; set; }
        public ICommand CreatePmCommand { get; set; }
        public bool IsActionPopupOpen { get; set; }
        public ICommand OpenActionPopupOpenCommand { get; set; }
        public ObservableCollection<PreventiveMaintenanceDto> PreventiveRecordsDtos { get; set; }
        private ObservableCollection<PreventiveRecordViewModel> _preventiveRecords { get; set; } 
        public ObservableCollection<PreventiveRecordViewModel> PreventiveRecords 
        {
            get => _preventiveRecords;
            set
            {
                _preventiveRecords = value;
                if (value != null)
                {
                       FilterPreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>(value);
                }
                OnPropertyChanged(nameof(PreventiveRecords));
            }
        }
        public ObservableCollection<PreventiveRecordViewModel> FilterPreventiveRecords { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public PreventiveMaintenanceStore? PreventiveMaintenanceStore { get; set; }
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
        public ICommand SearchCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public bool IsMenuFilterOpen { get; set; }

        public ICommand OpenFilterCommand { get; set; }
        public ICommand ApplyFilterCommand { get; set; }
        public PreventiveMaintenanceViewModel()
        {
            OpenFilterCommand = new RelayCommand(() => IsMenuFilterOpen = !IsMenuFilterOpen);
            AddFilterCommand = new RelayCommand(AddFilter);
            ClearAllFiltersCommand = new RelayCommand(ClearAllFilters);
            ApplyFilterCommand = new RelayCommand(ApplyFilters);

            AvailableFields = new ObservableCollection<string> { "Prochaine échéance" };
            AvailableOperators = new ObservableCollection<string> { "Est", "N'est pas" };
            

            CreatePmCommand = new RelayCommand(() => CreatePm());
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            SearchCommand = new RelayCommand(() => Search());
            PreventiveMaintenanceStore = new PreventiveMaintenanceStore();
            PreventiveMaintenanceStore.PreventiveMaintenanceCreated += PreventiveMaintenanceStore_PreventiveMaintenanceCreated;
           
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            RemoveCommand = new RelayCommand(async () => await Remove());

        }

        private void ClearAllFilters()
        {
            FilterPreventiveRecords.Clear();
            FilterPreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>(PreventiveRecords);
        }

        private void AddFilter()
        {
            Filters ??= new ObservableCollection<FilterPmViewModel>();
            Filters.Add(new FilterPmViewModel());
        }
        private void ApplyFilters()
        {
            if(Filters is null || Filters.Count <= 0)
            {
                return;
            }
            foreach (var filter in Filters)
            {
                if (filter.Field == "Prochaine échéance" && filter.Value is DateRangeModel dateRange)
                {
                    if(dateRange.StartDate > dateRange.EndDate)
                    {
                        IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "La date de début doit être inférieure à la date de fin"));
                        return;
                    }

                    FilterPreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>
                        (PreventiveRecords.Where(task => task.NextDueDate >= dateRange.StartDate && task.NextDueDate <= dateRange.EndDate));
                }
            }
           
        }
        private async Task Remove()
        {
            if (FilterPreventiveRecords is null || FilterPreventiveRecords.Count <= 0)
                return;
            List<PreventiveRecordViewModel>? itemToRemove = new();
            foreach (var item in FilterPreventiveRecords)
            {
                if (item.IsSelected)
                {
                    itemToRemove.Add(item);
                }
            }
            if (itemToRemove.Count <= 0)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Info, "Aucun PM sélectionné"));
                return;

            }
            var dialog = new ConfirmationWindow("Supprimer", "Voulez-vous vraiment supprimer les PM sélectionnés ?");
            dialog.ShowDialog();
            if (dialog.Confirmed)
            {
                List<Guid?> Ids = itemToRemove.Select(x => x.PreventiveMaintenanceDto?.Id).ToList();
                var response = await AppServices.ServiceManager.PreventiveMaintenanceService.BulkDeletePreventiveMaintenance(Ids,true);
                if(response is ApiOkResponse<string>)
                {
                    foreach (var item in itemToRemove)
                    {
                        PreventiveRecords.Remove(item);
                        FilterPreventiveRecords.Remove(item);
                        PreventiveRecordsDtos.Remove(item.PreventiveMaintenanceDto!);
                    }
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "PM supprimé avec succès"));
                }
                else if (response is ApiBadRequestResponse badResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResponse.Message));
                }
            }
        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || PreventiveRecords is null || PreventiveRecords.Count <= 0)
            {
                // Make filtered list the same
                FilterPreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>(PreventiveRecords ?? Enumerable.Empty<PreventiveRecordViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterPreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>(
                PreventiveRecords.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }
        private void PreventiveMaintenanceStore_PreventiveMaintenanceCreated(PreventiveMaintenanceDto? dto)
        {
            if (dto != null)
            {
                PreventiveRecordsDtos.Add(dto);
                PreventiveRecords.Add(new PreventiveRecordViewModel(dto));
                FilterPreventiveRecords.Add(new PreventiveRecordViewModel(dto));
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var response = await AppServices.ServiceManager.PreventiveMaintenanceService.GetAllPreventiveMaintenancesAsync(false);
                if (response.Success && response is ApiOkResponse<IEnumerable<PreventiveMaintenanceDto>> result)
                {
                    if(result.Result != null)
                    {
                        PreventiveRecordsDtos = new ObservableCollection<PreventiveMaintenanceDto>(result.Result);
                        PreventiveRecords = new ObservableCollection<PreventiveRecordViewModel>(PreventiveRecordsDtos.Select(x => new PreventiveRecordViewModel(x)));
                    }
                }
                else if (response is ApiBadRequestResponse badResponse)
                {
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badResponse.Message));
                }
              

            }
            catch (Exception ex)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, ex.Message));
            }
        }

        private void CreatePm()
        {
            IoContainer.Application.GoToPage(ApplicationPage.NewPreventiveMaintenance , new NewPreventiveMaintenanceViewModel { PreventiveMaintenanceStore = PreventiveMaintenanceStore });
        }
    }
}
