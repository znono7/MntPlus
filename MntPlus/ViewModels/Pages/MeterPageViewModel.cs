using Entities;
using Shared;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class MeterPageViewModel : BaseViewModel
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
                //if (string.IsNullOrEmpty(SearchText))
                    // Search to restore messages
                    Search();
            }
        }
        public List<MeterDto> MeterDtos { get; set; } 
        private ObservableCollection<MeterViewModel>? _meters;
        public ObservableCollection<MeterViewModel>? FilterMeters { get; set; }
        public ObservableCollection<MeterViewModel>? Meters
        {
            get => _meters;
            set
            {
                _meters = value;
                if (_meters != null)
                    FilterMeters = new ObservableCollection<MeterViewModel>(_meters);
                OnPropertyChanged("Meters");
            }
        }  

        public ICommand ViewMeterCommand { get; set; }
        public ICommand OpenControlCommand { get; set; }

        public ICommand OpenActionPopupOpenCommand { get; set; }

        public bool IsActionPopupOpen { get; set; }

        public bool AddMeterPopupIsOpen { get; set; }

        public AddMeterViewModel AddMeterViewModel { get; set; }
        public ReadingMeterViewModel ReadingMeterViewModel { get; set; }

        public bool IsReadingMeterPopupOpen { get; set; }
        public bool DimmableOverlayVisible { get; set; }

        public MeterStore? MeterStore { get; set; }
        public MeterPageViewModel()
        {
            OpenControlCommand = new RelayCommand(OpenControl);
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            ViewMeterCommand = new RelayParameterizedCommand((p) => ViewMeter(p));
            MeterStore = new MeterStore();
            MeterStore.MeterCreated += MeterStore_MeterCreated; 
            MeterStore.MeterUpdated += MeterStore_MeterUpdated;
            MeterStore.MeterDeleted += MeterStore_MeterDeleted;

            _ = LoadData();

        }

        private void MeterStore_MeterDeleted(MeterDto? dto)
        {
            var meter = Meters?.FirstOrDefault(m => m.MeterDto.Id == dto?.Id);
            if (meter != null)
            {
                Meters?.Remove(meter);
                FilterMeters?.Remove(meter);
            }
        }

        //private async Task RemoveBulkMeters()
        //  {
        //      if(FilterMeters == null || FilterMeters.Count == 0)
        //      {
        //          await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Pas de compteurs à supprimer"));
        //          return;
        //      }
        //      var selectedMeters = FilterMeters.Where(m => m.IsChecked).ToList();
        //      if (selectedMeters.Count == 0)
        //      {
        //          await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner un compteur pour supprimer"));
        //          return;
        //      }
        //      var response = await AppServices.ServiceManager.MeterService.RemoveBulkMetersAsync(selectedMeters.Select(m => m.MeterDto.Id).ToList());
        //      if (response.Success)
        //      {
        //          foreach (var meter in selectedMeters)
        //          {
        //              Meters.Remove(meter);
        //              FilterMeters.Remove(meter);
        //          }
        //      }
        //      else if (response is ApiBadRequestResponse badRequestResponse)
        //      {
        //          await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
        //      }

        //  }

        private async Task LoadData()
        {
            var response = await AppServices.ServiceManager.MeterService.GetAllMetersAsync(false);
            if (response.Success && response is ApiOkResponse<IEnumerable<MeterDto>> result)
            {
                MeterDtos = new List<MeterDto>(result.Result!);
                Meters = new ObservableCollection<MeterViewModel>(MeterDtos.Select(m => new MeterViewModel(m)));
            }else if (response is ApiBadRequestResponse badRequestResponse)
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, badRequestResponse.Message));
            }
              
        }

        private void MeterStore_MeterUpdated(MeterDto? dto)
        {
            var meter = Meters?.FirstOrDefault(m => m.MeterDto.Id == dto?.Id);
            if (meter != null)
            {
                int index = Meters!.IndexOf(meter);
                Meters[index] = new MeterViewModel(dto!);
                FilterMeters = new ObservableCollection<MeterViewModel>(Meters);

            }

        }

        private void MeterStore_MeterCreated(MeterDto? dto)
        {
            if (dto != null)
            {
                Meters ??= new ObservableCollection<MeterViewModel>();
                FilterMeters ??= new ObservableCollection<MeterViewModel>();
                Meters?.Add(new MeterViewModel(dto));
                FilterMeters.Add(new MeterViewModel(dto));
            }
        }

        private void ViewMeter(object? p)
        {
            if (p is MeterViewModel meterViewModel)
            {
                ReadingMeterViewModel = new ReadingMeterViewModel(meterViewModel.MeterDto, meterViewModel.NextReading, MeterStore);
                ReadingMeterViewModel.CloseAction = CloseControl;
                ReadingMeterViewModel.EditAction = EditMeter;
                DimmableOverlayVisible = true;
                IsReadingMeterPopupOpen = true;
            }
        }

        private async Task EditMeter(MeterDto dto)
        {
            AddMeterViewModel = new AddMeterViewModel(MeterStore,dto);
            AddMeterViewModel.CloseAction = CloseControl;
            IsReadingMeterPopupOpen = false;
            DimmableOverlayVisible = true;
            AddMeterPopupIsOpen = true;
            await Task.Delay(1);
        }

        private void OpenControl()
        {
            IsReadingMeterPopupOpen = false;
            AddMeterViewModel = new AddMeterViewModel(MeterStore);
            AddMeterViewModel.CloseAction = CloseControl;
            DimmableOverlayVisible = true;
            AddMeterPopupIsOpen = true;

        }
        private async Task CloseControl()
        {
            AddMeterPopupIsOpen = false;
            DimmableOverlayVisible = false;
            IsReadingMeterPopupOpen = false;
            await Task.Delay(1);
        }
        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || Meters is null || Meters.Count <= 0)
            {
                // Make filtered list the same
                FilterMeters = new ObservableCollection<MeterViewModel>(Meters ?? Enumerable.Empty<MeterViewModel>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterMeters = new ObservableCollection<MeterViewModel>(
                Meters.Where(item => item.MeterDto.Name is not null && item.MeterDto.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }


    }
}
