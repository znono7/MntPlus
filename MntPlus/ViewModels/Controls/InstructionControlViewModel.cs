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
          
        }
    }
}
