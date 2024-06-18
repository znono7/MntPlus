using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class TaskRecord : BaseViewModel
    {

        public CheckListItemDto CheckListItemDto { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }

        public TaskRecord(CheckListItemDto checkListItemDto)
        {
            CheckListItemDto = checkListItemDto;
            Description = checkListItemDto.Description;
            Order = checkListItemDto.Order;

        }


    }
}
