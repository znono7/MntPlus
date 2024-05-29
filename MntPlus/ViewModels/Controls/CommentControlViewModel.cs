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
    public class CommentControlViewModel : BaseViewModel
    {
        public string? Comment { get; set; }
        public ICommand RemoveCommand { get; set; }
        public Func<CommentControlViewModel, Task>? RemoveItemFunc { get; set; }

        public bool Working { get; set; }
        public WorkOrderHistoryDto? WorkOrderHistoryDto { get; }

        public CommentControlViewModel(WorkOrderHistoryDto? workOrderHistoryDto)
        {
            RemoveCommand = new RelayCommand(async () => await Remove());
            WorkOrderHistoryDto = workOrderHistoryDto;
        }

        private async Task Remove()
        {
            if(RemoveItemFunc == null)
                return;
            await RemoveItemFunc(this);
        }
    }
}
