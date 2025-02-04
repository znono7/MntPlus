﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class WorkOrderItemsViewModel : BaseViewModel
    {
        public WorkOrderDto? WorkOrderDto { get; set; } 
        public string WorkOrderNumber => AddDynamicLeadingZeros(WorkOrderDto?.Number ?? 0); 
        public string WorkOrderName { get; set; } 
        public string PriorityBackground { get; set; }  = "000000";
          
        private string _orderWorkPriority = "3";
        public string Priority { get; set; }
        public string OrderWorkPriority
        {
            get => _orderWorkPriority;
            set
            {
                _orderWorkPriority = value;
                OnPropertyChanged(nameof(OrderWorkPriority));
                switch (_orderWorkPriority)
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
                        default:
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
                    case "Approuvé":
                        ForgroundColorStat = "1C62B9";
                        break;
                    case "En attente":
                        ForgroundColorStat = "7B68EE";
                        break;
                    case "En service":
                        ForgroundColorStat = "ef6a00";
                        break;
                    case "Complet":
                        ForgroundColorStat = "c22528";
                        break;
                    case "Ouvrir":
                        ForgroundColorStat = "429b1f";
                        break;
                    case "Non spécifique":
                        ForgroundColorStat = "595959";
                        break;
                }
            }
        }

        public string WorkAssigned { get; set; }
        public string WorkAssignedName { get; set; }

        public DateTime? AssetCommissionDate { get ; set; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("dd/MM/yyyy");

        public string? AssetName => WorkOrderDto?.Asset?.Name ?? string.Empty;

        public ICommand ViewOrderWorkCommand { get; set; }

        public Func<WorkOrderDto?, Task>? ViewOrderWork { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public WorkOrderItemsViewModel(WorkOrderDto? workOrderDto)
        {
            WorkOrderDto = workOrderDto;
            WorkOrderName = WorkOrderDto?.Name ?? string.Empty;
            //OrderWorkPriority = WorkOrderDto?.Priority ?? string.Empty;
            Priority = SetOrderWorkPriority(WorkOrderDto?.Priority);
            PriorityBackground = SetBackgroundOrderWorkPriority(WorkOrderDto?.Priority);
            Type = WorkOrderDto?.Type ?? string.Empty;
            WorkStat = WorkOrderDto?.Status ?? string.Empty;
            AssetCommissionDate = WorkOrderDto?.DueDate;
            if(WorkOrderDto?.UserAssignedTo is not null)
            {
                WorkAssigned = "user";
                WorkAssignedName = $"{WorkOrderDto?.UserAssignedTo?.FullName}" ?? string.Empty;
            }else if(WorkOrderDto?.TeamAssignedTo is not null)
            {
                WorkAssigned = "team";
                WorkAssignedName = WorkOrderDto?.TeamAssignedTo?.Name ?? string.Empty;
            }

            ViewOrderWorkCommand = new RelayCommand(async () =>  await ViewOrderWorkDetail(WorkOrderDto) );
                
           
        }
        private string SetOrderWorkPriority(string? priority)
        {
            switch (priority)
            {
                case "1":
                    return "Haute";
                case "2":
                    return "Moyenne";
                case "3":
                    return "Basse";
                case "Aucune":
                    return "Aucune";
                default:
                    return "Aucune";
            }
        }
        private string SetBackgroundOrderWorkPriority(string? priority)
        {
            switch (priority)
            {
                case "1":
                    return "c22528";
                case "2":
                    return "ef6a00";
                case "3":
                    return "429b1f";
                case "Aucune":
                    return "53667B";
                default:
                    return "53667B";
            }
        }

        private async Task ViewOrderWorkDetail(WorkOrderDto? workOrderDto)
        {
            if (ViewOrderWork is not null)
            {
                await ViewOrderWork(workOrderDto);
            }
        }
        public string AddDynamicLeadingZeros(int number)
        {
            // Get the number of digits in the number
            int numberOfDigits = number.ToString().Length;

            // Calculate the total length after adding zeros
            int totalLength = numberOfDigits * 2 + 1;

            // Pad the number with leading zeros
            return number.ToString().PadLeft(totalLength, '0');
        }
    }
}
