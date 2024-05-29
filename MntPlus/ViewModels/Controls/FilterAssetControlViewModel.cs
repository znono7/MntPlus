using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class FilterAssetControlViewModel : BaseViewModel
    {
        public bool IsCategoryPopupOpen { get; set; }
        public bool IsStatutPopupOpen { get; set; }
        public bool IsSerialNumberPopupOpen { get; set; }
        public bool IsModelPopupOpen { get; set; }
        public bool IsMakePopupOpen { get; set; }
        public bool IsDayeServicePopupOpen { get; set; }
        public bool IsDayeCreatedPopupOpen { get; set; }



        public ICommand CategoryPopupCommand { get; set; }
        public ICommand StatutPopupCommand { get; set; }
        public ICommand SerialNumberPopupCommand { get; set; }
        public ICommand ModelPopupCommand { get; set; }
        public ICommand MakePopupCommand { get; set; }
        public ICommand DayeServicePopupCommand { get; set; }
        public ICommand DayeCreatedPopupCommand { get; set; }
        public EquipmentCategories? EquipmentCategories { get; set; }
        public AssetStatuses AssetStatuses { get; private set; }
        private AssetStatus? selectedAssetStatut { get; set; }
         public AssetStatus? SelectedAssetStatut
        {
            get => selectedAssetStatut;
            set
            {
                selectedAssetStatut = value;
                FilterByStatutCommand.Execute(null);
                IsStatutPopupOpen = false;
                OnPropertyChanged(nameof(SelectedAssetStatut));

            }
        }
        private EquipmentCategory? selectedEquipmentCategory { get; set; }
        public EquipmentCategory? SelectedEquipmentCategory
        {
            get => selectedEquipmentCategory;
            set
            {
                selectedEquipmentCategory = value;
                FilterByCategoryCommand.Execute(null);
                IsCategoryPopupOpen = false;
                OnPropertyChanged(nameof(SelectedEquipmentCategory));
            }

        }
        public ICommand FilterByCategoryCommand { get; set; }
        public Func<EquipmentCategory, Task>? FilterByCategoryFonction { get; set; }

        public ICommand FilterByStatutCommand { get; set; }
        public Func<AssetStatus, Task>? FilterByStatutFonction { get; set; }

        public string? SerialNumber { get; set; }
        public ICommand FilterSerialNumberCommand { get; set; }
        public Func<string?, Task>? FilterBySerialNumberFonction { get; set; }

        public string? Model { get; set; }
        public ICommand FilterByModelCommand { get; set; }
        public Func<string?, Task>? FilterByModelFonction { get; set; }

        public string? Make { get; set; }
        public ICommand FilterByMakeCommand { get; set; }
        public Func<string?, Task>? FilterByMakeFonction { get; set; }

        public ICommand OpenMenuFromDateCommand { get; set; }
        public bool IsMenuFromDateOpen { get; set; }
        public DateTime? FromDateService { get; set; }
        public string? FromDateServiceText => FromDateService?.ToString("dd/MM/yyyy");

        private DateTime? _toDateService { get; set; }
        public DateTime? ToDateService { get => _toDateService; set
            {
                _toDateService = value;
                FilterByDateServiceCommand.Execute(null);
                IsDayeServicePopupOpen = false;
                OnPropertyChanged(nameof(ToDateService));
                OnPropertyChanged(nameof(ToDateServiceText));
            }
        }
        public string? ToDateServiceText => ToDateService?.ToString("dd/MM/yyyy");
        public  ICommand OpenMenuToDateCommand { get; set; }
        public bool IsMenuToDateOpen { get; set; }

        public Func<DateTime?, DateTime?, Task>? FilterByDateServiceFonction { get; set; }
        public ICommand FilterByDateServiceCommand { get; set; }

        public bool IsFromDayeCreatedPopupOpen { get; set; }
        public bool IsToDayeCreatedPopupOpen { get; set; }
        public DateTime? FromDateCreated { get; set; }
        public string? FromDateCreatedText => FromDateCreated?.ToString("dd/MM/yyyy");
        public ICommand OpenMenuFromDateCreatedCommand { get; set; }
        private DateTime? _toDateCreated { get; set; }
        public DateTime? ToDateCreated { get => _toDateCreated; set
            {
                _toDateCreated = value;
                FilterByDateCreatedCommand.Execute(null);
                IsDayeCreatedPopupOpen = false;
                OnPropertyChanged(nameof(ToDateCreated));
                OnPropertyChanged(nameof(ToDateCreatedText));
            }
        }
        public string? ToDateCreatedText => ToDateCreated?.ToString("dd/MM/yyyy");
        public ICommand OpenMenuToDateCreatedCommand { get; set; }
        public Func<DateTime?, DateTime?, Task>? FilterByDateCreatedFonction { get; set; }
        public ICommand FilterByDateCreatedCommand { get; set; }

        public ICommand ResetCommand { get; set; }
        public Func<Task>? ResetFunction { get; set; }

        public FilterAssetControlViewModel()
        {
            CategoryPopupCommand = new RelayCommand(() => IsCategoryPopupOpen = !IsCategoryPopupOpen);
            StatutPopupCommand = new RelayCommand(() => IsStatutPopupOpen = !IsStatutPopupOpen);
            SerialNumberPopupCommand = new RelayCommand(() => IsSerialNumberPopupOpen = !IsSerialNumberPopupOpen);
            ModelPopupCommand = new RelayCommand(() => IsModelPopupOpen = !IsModelPopupOpen);
            MakePopupCommand = new RelayCommand(() => IsMakePopupOpen = !IsMakePopupOpen);
            DayeServicePopupCommand = new RelayCommand(() => IsDayeServicePopupOpen = !IsDayeServicePopupOpen);
            OpenMenuFromDateCommand = new RelayCommand(() => IsMenuFromDateOpen = !IsMenuFromDateOpen);
            OpenMenuToDateCommand = new RelayCommand(() => IsMenuToDateOpen = !IsMenuToDateOpen);
            DayeCreatedPopupCommand = new RelayCommand(() => IsDayeCreatedPopupOpen = !IsDayeCreatedPopupOpen);
            OpenMenuFromDateCreatedCommand = new RelayCommand(() => IsFromDayeCreatedPopupOpen = !IsFromDayeCreatedPopupOpen);
            OpenMenuToDateCreatedCommand = new RelayCommand(() => IsToDayeCreatedPopupOpen = !IsToDayeCreatedPopupOpen);
            EquipmentCategories = new EquipmentCategories();
            AssetStatuses = new AssetStatuses();
            FilterByCategoryCommand = new RelayCommand(async () => await FilterByCategory());
            FilterByStatutCommand = new RelayCommand(async () => await FilterByStatut());
            FilterSerialNumberCommand = new RelayCommand(async () => await FilterBySerialNumber());
            FilterByModelCommand = new RelayCommand(async () => await FilterByModel());
            FilterByMakeCommand = new RelayCommand(async () => await FilterByMake());
            FilterByDateServiceCommand = new RelayCommand(async () => await FilterByDateService());
            FilterByDateCreatedCommand = new RelayCommand(async () => await FilterByDateCreated());
            ResetCommand = new RelayCommand(async () => await Reset());

        }

        private async Task Reset()
        {
            if(ResetFunction != null)
            {
                await ResetFunction();
            }
            
        }

        private async Task FilterByDateCreated()
        {
            if(FilterByDateCreatedFonction != null && FromDateCreated != null && ToDateCreated != null)
            {
                await FilterByDateCreatedFonction(this.FromDateCreated, this.ToDateCreated);
            }
            
        }

        private async Task FilterByDateService()
        {
            if(FilterByDateServiceFonction != null && FromDateService != null && ToDateService != null)
            {
                await FilterByDateServiceFonction(this.FromDateService, this.ToDateService);
            }
            
        }

        private async Task FilterByMake()
        {
            if(FilterByMakeFonction != null && Make != null)
            {
                await FilterByMakeFonction(this.Make);
            }
            
        }

        private async Task FilterByModel()
        {
            if(FilterByModelFonction != null && Model != null)
            {
                await FilterByModelFonction(this.Model);
            }
            
        }

        private async Task FilterBySerialNumber()
        {
            if(FilterBySerialNumberFonction != null && SerialNumber != null)
            {
                await FilterBySerialNumberFonction(this.SerialNumber);
            }
            
        }

        private async Task FilterByStatut()
        {
            if(FilterByStatutFonction != null && SelectedAssetStatut != null)
            {
                await FilterByStatutFonction(this.SelectedAssetStatut);
            }
            
        }

        private async Task FilterByCategory()
        {
            if(FilterByCategoryFonction != null && SelectedEquipmentCategory != null)
            {
                await FilterByCategoryFonction(this.SelectedEquipmentCategory);

            }
        }
    }
}
