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
    public class ChecklistsViewModel : BaseViewModel
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
        public BaseViewModel checkListControlViewModel { get; set; }
         
        private ObservableCollection<CheckListDto> CheckListDtos { get; set; }
        private ObservableCollection<CheckListRecord> _checkLists { get; set; }
        public ObservableCollection<CheckListRecord> CheckLists
        {
            get => _checkLists;
            set
            {
                _checkLists = value;
                if (value != null)
                {
                    FilterCheckLists = new ObservableCollection<CheckListRecord>(value);
                }
                OnPropertyChanged(nameof(CheckLists));
            }
        }
        public ObservableCollection<CheckListRecord> FilterCheckLists { get; set; }
        public bool IsCheckListVisible { get; set; } = false;
        public bool DimmableOverlayVisible { get; set; } = false;
        public ICommand ToggleCheckListCommand { get; set; }
        public CheckListStore CheckListStore { get; set; }
        public ICommand OpenActionPopupOpenCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ViewCheckListCommand { get; set; }

        public bool IsActionPopupOpen { get; set; }

        public ChecklistsViewModel()
        {
            CheckLists = new ObservableCollection<CheckListRecord>();
            _ = GetData();
            ToggleCheckListCommand = new RelayCommand(() => ToggleCheckList());
            OpenActionPopupOpenCommand = new RelayCommand(() => IsActionPopupOpen = !IsActionPopupOpen);
            DeleteCommand = new RelayCommand(() => DeleteCheckList());
            ViewCheckListCommand = new RelayParameterizedCommand((p) => ViewCheckList(p));
        }

        private void ViewCheckList(object? p)
        {
            if (p is CheckListRecord checkListRecord)
            {
                CheckListStore = new CheckListStore();
                CheckListStore.CheckListUpdated += CheckListStore_CheckListUpdated;
                checkListControlViewModel = new UpdateCheckListControlViewModel(CheckListStore, checkListRecord.CheckListDto)
                {
                    CloseAction = CloseControl
                };
                IsCheckListVisible = true;
                DimmableOverlayVisible = true;
            }
        }

        private void CheckListStore_CheckListUpdated(CheckListDto? dto)
        {
            var checkListRecord = CheckLists.FirstOrDefault(x => x.CheckListDto.Id == dto!.Id);
            //get the index of the record
            var index = CheckLists.IndexOf(checkListRecord!);
            //and update the record
            if (index == -1)
            {
                CheckLists.Add(new CheckListRecord(dto!));
            }
            else
            {
                CheckLists[index] = new CheckListRecord(dto!);
             
            }
           
        }

        private void DeleteCheckList()
        {
            var selectedCheckLists = FilterCheckLists.Where(x => x.IsChecked).ToList();
            if (selectedCheckLists.Count <=0)
            {
                IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez sélectionner une liste de contrôle à supprimer"));
                return;
            }
          
            var Dialog = new ConfirmationWindow("Supprimer les listes de contrôle", "Êtes-vous sûr de vouloir supprimer les listes de contrôle sélectionnées ?");
            Dialog.ShowDialog();
            if (Dialog.Confirmed == true)
            {
                var ids = selectedCheckLists.Select(x => x.CheckListDto.Id).ToList();
                _ = DeleteCheckLists(ids);
            }
        }

        private async Task DeleteCheckLists(List<Guid> ids)
        {
            var response = await AppServices.ServiceManager.TaskListService.BulkDeleteCheckLists(ids,true);
            if (response is not null && response.Success)
            {
                var deletedCheckLists = CheckLists.Where(x => ids.Contains(x.CheckListDto.Id)).ToList();
                foreach (var checkList in deletedCheckLists)
                {
                    CheckLists.Remove(checkList);
                    FilterCheckLists.Remove(checkList);
                }
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Liste de contrôle supprimée avec succès"));
            }
            else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de la liste de contrôle"));
            }
        }

        private async Task GetData()
        {
            var response = await AppServices.ServiceManager.TaskListService.GetCheckLists(false); 
            if (response is not null && response.Success && response is ApiOkResponse<IEnumerable<CheckListDto>> resule)
            {
                CheckListDtos = new ObservableCollection<CheckListDto>(resule.Result!);
                CheckLists = new ObservableCollection<CheckListRecord>(CheckListDtos.Select(x => new CheckListRecord(x)));
            }else
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la récupération des listes de contrôle"));
            }

        }
        private void ToggleCheckList()
        {
            CheckListStore = new CheckListStore(); 
            CheckListStore.CheckListCreated += CheckListStore_CheckListCreated;
            checkListControlViewModel = new CheckListControlViewModel(CheckListStore)
            {
                CloseAction = CloseControl
            };
            IsCheckListVisible = !IsCheckListVisible;
            DimmableOverlayVisible = IsCheckListVisible;
        }

        private void CheckListStore_CheckListCreated(CheckListDto? obj)
        {
            CheckLists ??= new ObservableCollection<CheckListRecord>();
            CheckLists.Add(new CheckListRecord(obj!));
            FilterCheckLists = new ObservableCollection<CheckListRecord>(CheckLists);
        }

        private async Task CloseControl()
        {
            IsCheckListVisible = false;
            DimmableOverlayVisible = false;
            await Task.Delay(1);
        }

        private void Search()
        {
            // Make sure we don't re-search the same text
            if ((string.IsNullOrEmpty(mLastSearchText) && string.IsNullOrEmpty(SearchText)) ||
                string.Equals(mLastSearchText, SearchText))
                return;

            if (string.IsNullOrEmpty(SearchText) || CheckLists is null || CheckLists.Count <= 0)
            {
                // Make filtered list the same
                FilterCheckLists = new ObservableCollection<CheckListRecord>(CheckLists ?? Enumerable.Empty<CheckListRecord>());

                // Set last search text
                mLastSearchText = SearchText;

                return;
            }
            FilterCheckLists = new ObservableCollection<CheckListRecord>(
                CheckLists.Where(item => item.Name is not null && item.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            // Set last search text
            mLastSearchText = SearchText;

        }

    }

    public class CheckListRecord : BaseViewModel
    {
        public CheckListDto CheckListDto { get; set; }
        private bool _isChecked { get; set; }
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public CheckListRecord(CheckListDto checkListDto)
        {
            CheckListDto = checkListDto;
            Description = checkListDto.Description;
            Name = checkListDto.Name;
            IsChecked = false;
        }

    }
}
 