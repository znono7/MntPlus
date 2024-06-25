using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class RequestItemViewModel : BaseViewModel
    {
        public string WorkOrderNumber => AddDynamicLeadingZeros(RequestDto?.Number ?? 0);
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

        public string ForgroundColorStat { get; set; } = "429b1f";

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
                    case "Refuser":
                        ForgroundColorStat = "595959";
                        break;
                }
            }
        }

        public string WorkAssigned { get; set; }
        public string WorkAssignedName { get; set; }

        public DateTime? AssetCommissionDate { get; set; }
        public string? ShortAssetCommissionDate => AssetCommissionDate?.ToString("dd/MM/yyyy");

        public string? AssetName => RequestDto?.Asset?.Name ?? string.Empty;

        public ICommand ViewOrderWorkCommand { get; set; }

        public Func<RequestDto?, Task>? ViewOrderWork { get; set; }

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

        public RequestDto? RequestDto { get; set; }

        public RequestItemViewModel(RequestDto? requestDto)
        {
            RequestDto = requestDto;
            WorkOrderName = RequestDto?.Name ?? string.Empty;
            OrderWorkPriority = RequestDto?.Priority ?? string.Empty;
            Type = RequestDto?.Type ?? string.Empty;
            WorkStat = RequestDto?.Status ?? string.Empty;
            AssetCommissionDate = RequestDto?.DueDate;
            if (RequestDto?.UserAssignedTo is not null)
            {
                WorkAssigned = "user";
                WorkAssignedName = $"{RequestDto?.UserAssignedTo?.FullName}" ?? string.Empty;
            }
            else if (RequestDto?.TeamAssignedTo is not null)
            {
                WorkAssigned = "team";
                WorkAssignedName = RequestDto?.TeamAssignedTo?.Name ?? string.Empty;
            }

            ViewOrderWorkCommand = new RelayCommand(async () => await ViewOrderWorkDetail(RequestDto));

        }

        private async Task ViewOrderWorkDetail(RequestDto? workOrderDto)
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
