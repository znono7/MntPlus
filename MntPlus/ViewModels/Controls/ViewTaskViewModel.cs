using Entities;
using Microsoft.VisualBasic;
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
    public class ViewTaskViewModel : BaseViewModel
    {
        public InstructionControlViewModel InstructionControlViewModel { get; set; }
        public CommentControlViewModel CommentControlViewModel { get; set; }

        public bool IsCommentControlOpen { get; set; }
        public bool IsMenuInstructionOpen { get; set; }
        public bool IsMenuPrioprityOpen { get; set; }
        public ICommand OpenCommentControlCommand { get; set; }
        public ICommand OpenMenuInstructionCommand { get; set; }

        public ICommand OpenMenuPriorityCommand { get; set; }
        public ICommand HighPriorityCommand { get; set; }
        public ICommand MediumPriorityCommand { get; set; }
        public ICommand LowPriorityCommand { get; set; }
        public ICommand OpenMenuDueDateCommand { get; set; }
        public ICommand OpenMenuTypeCommand { get; set; }
        public ICommand OpenMenuAssignedCommand { get; set; }
        public ICommand OpenMenuStatCommand { get; set; }
        public ICommand OpenStatCommand { get; set; }
        public ICommand InProgressCommand { get; set; }
        public ICommand CompleteCommand { get; set; }


        public string ForgroundColor { get; set; } = "429b1f";
        private string _orderWorkPriority = "Faible";
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                if (_orderWorkPriority == value)
                {
                    return;
                }
                _orderWorkPriority = value;
                switch (value)
                {
                    case "Haute":
                        ForgroundColor = "c22528";
                        break;
                    case "Moyenne":
                        ForgroundColor = "ef6a00";
                        break;
                    case "Basse":
                        ForgroundColor = "429b1f";
                        break;
                }
                IsMenuPrioprityOpen = false;
            }
        }

        public string TagStat { get; set; } = "statopen";
        private string _WorkStat = "Ouvrir";
        public string WorkStat
        {
            get => _WorkStat;
            set
            {
                if (_WorkStat == value)
                {
                    return;
                }
                _WorkStat = value;
                switch (value)
                {
                    case "Ouvrir":
                        ForgroundColorStat = "429b1f";
                        TagStat = "statopen";
                        break;
                    case "En Cours":
                        ForgroundColorStat = "ef6a00";
                        TagStat = "statinprogress";
                        IsMenuStatOpen = false;

                        break;
                    case "Complété":
                        ForgroundColorStat = "c22528";
                        TagStat = "statcomplete";
                        IsMenuStatOpen = false;

                        break;
                }
                IsMenuStatOpen = false;
            }
        }

        private DateTime _dueDate = DateTime.Today;
        public DateTime DueDate { get => _dueDate; set => _dueDate = value.Date; }
        public string? ShortDueDate => DueDate.ToShortDateString();

        public bool IsMenuDueDateOpen { get; set; }
        public bool IsMenuTypeOpen { get; set; }
        public bool IsMenuAssignedOpen { get; set; }
        public string ForgroundColorStat { get;  set; } = "429b1f";
        public bool IsMenuStatOpen { get; set; }

        public string WorkType { get; set; } = "Prévu";

        public TextEntryViewModel TaskName { get; set; }
        public WorkOrderDto? Dto { get; set; }

        public string? WorkAsset { get; set; }

        public ICommand ClosePopupCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RemoveInstructionCommand { get; set; }
        public bool SaveIsRunning { get; set; }
        public bool DeleteIsRunning { get; set; }
        public bool DimmableOverlayVisible { get; set; }
        public Func<Task> ClosePopupAction { get; set; }

        public ObservableCollection<InstructionDto> InstructionDtos { get; set; } 
        public ObservableCollection<InstructionDto> CompleteWork { get; set; }
        public ObservableCollection<WorkOrderHistoryDto> WorkOrderHistoryDtos { get; set; }

        public bool IsForComplete { get; set; }
        public ViewTaskViewModel(WorkOrderDto? dto)
        {
            Dto = dto;
            WorkType = dto?.Type!;
            DueDate = dto?.DueDate ?? DateTime.Today;
            WorkStat = dto?.Status ?? "Ouvrir";
            OrderWorkPriority = dto?.Priority ?? "Basse";
            TaskName = new TextEntryViewModel { OriginalText = dto?.Name! };
            WorkAsset = dto?.Asset?.Name ?? string.Empty;
            OpenMenuPriorityCommand = new RelayCommand(() => IsMenuPrioprityOpen = !IsMenuPrioprityOpen);
            OpenMenuDueDateCommand = new RelayCommand(() => IsMenuDueDateOpen = !IsMenuDueDateOpen);
            OpenMenuTypeCommand = new RelayCommand(() => IsMenuTypeOpen = !IsMenuTypeOpen);
            OpenMenuAssignedCommand = new RelayCommand(() => IsMenuAssignedOpen = !IsMenuAssignedOpen);
            HighPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Haute");
            MediumPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Moyenne");
            LowPriorityCommand = new RelayCommand(() => OrderWorkPriority = "Basse");
            OpenMenuStatCommand = new RelayCommand(() => IsMenuStatOpen = !IsMenuStatOpen);
            OpenStatCommand = new RelayCommand(() => WorkStat = "Ouvrir");
            InProgressCommand = new RelayCommand(() => WorkStat = "En Cours");
            CompleteCommand = new RelayCommand(() => WorkStat = "Complété");
            ClosePopupCommand = new RelayCommand(async () => await ClosePopupAction());
            SaveCommand = new RelayCommand(async () => await Save());
            InstructionDtos = new ObservableCollection<InstructionDto>
            {
                new InstructionDto ( Guid.Empty,  "Description 1" ),
                new InstructionDto ( Guid.Empty,  "Description 2" ),
                new InstructionDto ( Guid.Empty,  "Description 3" )
            };
            if(InstructionDtos is null)
            {
                CompleteWork = new ObservableCollection<InstructionDto>
                {
                    new InstructionDto ( Guid.Empty,  "Travail Complet" ),
                };
                IsForComplete = true;
            }
            OpenMenuInstructionCommand = new RelayCommand( async() => await MenuInstruction());
            InstructionControlViewModel = new InstructionControlViewModel(dto?.Id);
            InstructionControlViewModel.CommitAction = MenuInstruction;

            RemoveInstructionCommand = new RelayParameterizedCommand(async(p) => await RemoveInstruction(p));


           

        }

        private async Task MenuComment()
        {
           
            IsCommentControlOpen = !IsCommentControlOpen;
            DimmableOverlayVisible = IsCommentControlOpen ? true : false;
            await Task.Delay(1);
        }
        private async Task RemoveInstruction(object? p)
        {
            var instruction = p as InstructionDto;
            if (instruction is null)
            {
                return;
            }
            
            await RunCommandAsync(() => DeleteIsRunning, async () =>
            {
                var Result = await AppServices.ServiceManager.InstructionService.DeleteInstruction(instruction.Id,false);
                if(Result is not null && Result is ApiOkResponse<InstructionDto> response)
                {
                   
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Instruction supprimée avec succès"));
                }
                else
                {
                    
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de la suppression de l'instruction"));
                }
            });
            InstructionDtos.Remove(instruction);
            await Task.Delay(1);
            
        }

        private async Task MenuInstruction()
        {
            IsMenuInstructionOpen = !IsMenuInstructionOpen;
            DimmableOverlayVisible = IsMenuInstructionOpen ? true : false;
            await Task.Delay(1);
        }

        private async Task Save()
        {
            SaveIsRunning = true;
            await Task.Delay(1000);
            SaveIsRunning = false;
        }
    }
}
