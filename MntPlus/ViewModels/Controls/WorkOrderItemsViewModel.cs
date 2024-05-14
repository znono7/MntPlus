using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class WorkOrderItemsViewModel 
    {
        public WorkOrderDto? WorkOrderDto { get; set; }

        public string WorkOrderNumber =>  $"00{WorkOrderDto?.Number}";
        public string WorkOrderName { get; set; }
        public string PriorityBackground { get; set; } 

        private string _orderWorkPriority = "3";
        public string Priority { get; set; }
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                _orderWorkPriority = value;
                switch (value)
                {
                    case "1":
                        PriorityBackground = "c22528";
                        Priority = "Haute";
                        break;
                    case "2":
                        PriorityBackground = "ef6a00";
                        Priority = "Moyenne";
                        break;
                    case "3":
                        PriorityBackground = "429b1f";
                        Priority = "Basse";
                        break;
                    case "Aucune":
                        PriorityBackground = "53667B";
                        Priority = "Aucune";
                        break;
                }
            }
        }
        public string Type { get; set; }

        public string ForgroundColorStat { get;  set; } = "429b1f";

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
                        break;
                    case "En Cours":
                        ForgroundColorStat = "ef6a00";
                        break;
                    case "Complété":
                        ForgroundColorStat = "32A53D";
                        break;
                        case "Fermé":
                        ForgroundColorStat = "A4342F";
                        break;
                        case "Annulé":
                        ForgroundColorStat = "464E47";
                        break;
                        case "En Attente":
                        ForgroundColorStat = "AFC2B1";
                        break;
                }
            }
        }

        public string WorkAssigned { get; set; }
        public string WorkAssignedName { get; set; }

        public DateTime? AssetCommissionDate { get ; set; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("d");

        public string? AssetName => WorkOrderDto?.Asset?.Name ?? string.Empty;

        public ICommand ViewOrderWorkCommand { get; set; }

        public Func<WorkOrderDto?, Task>? ViewOrderWork { get; set; }
        public WorkOrderItemsViewModel(WorkOrderDto workOrderDto)
        {
            WorkOrderDto = workOrderDto;
            WorkOrderName = WorkOrderDto?.Name ?? string.Empty;
            OrderWorkPriority = WorkOrderDto?.Priority ?? string.Empty;
            Type = WorkOrderDto?.Type ?? string.Empty;
            WorkStat = WorkOrderDto?.Status ?? string.Empty;
            AssetCommissionDate = WorkOrderDto?.DueDate;
            if(WorkOrderDto?.UserAssignedTo is not null)
            {
                WorkAssigned = "user";
                WorkAssignedName = $"{WorkOrderDto?.UserAssignedTo?.FirstName} {WorkOrderDto?.UserAssignedTo?.LastName}" ?? string.Empty;
            }else if(WorkOrderDto?.TeamAssignedTo is not null)
            {
                WorkAssigned = "team";
                WorkAssignedName = WorkOrderDto?.TeamAssignedTo?.Name ?? string.Empty;
            }

            ViewOrderWorkCommand = new RelayCommand(async () => await ViewOrderWork(WorkOrderDto));
                
           
        }
    }
}
