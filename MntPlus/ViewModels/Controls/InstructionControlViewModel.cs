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
    public class InstructionControlViewModel : BaseViewModel
    {

        public Func<Task> CommitAction { get; set; }

        public string Instruction { get; set; }

        public Guid? Id { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public bool Working { get; set; }

         
        public InstructionControlViewModel(Guid? id)
        {
            AddCommand = new RelayCommand(async () => await Add());
            CancelCommand = new RelayCommand(() => CommitAction());
            Id = id;
        }

        private async Task Add()
        {
            var result = default(bool);
           await RunCommandAsync(() => Working, async () =>
            {
                var Response = await AppServices.ServiceManager.InstructionService.CreateInstruction(new InstructionDtoForCreation(Instruction,Id));
                if(Response is not null && Response is ApiOkResponse<InstructionDto> response)
                {
                    result = true;
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Instruction ajoutée avec succès"));
                }
                else
                {
                    result = false;
                    await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Erreur lors de l'ajout de l'instruction"));

                }

            }).ContinueWith(t =>
            {
                if(result)
                {
                    CommitAction();
                }
             
            });
        }
    }
}
