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
    public class PartInventoryItem : BaseViewModel
    {
        public PartDto? Part { get; set; } 
        public Guid Id { get; set; } 
        public string? PartNumber { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public int? AvailableQuantity { get; set; }
        public bool IsChecked { get; set; }
       
        public string? Status { get; set; }

        public string? StatusColor
        {
            get
            {
                return Status switch
                {
                    "En Rupture de stock" => "FFAAAA",
                    "Stock Faible" => "FFD78D",
                    "En Stock" => "AAD4AA",
                    _ => "#000000"
                };
            }
        }

        public string? ForgroundStatusColor
        {
            get
            {
                return Status switch
                {
                    "En Rupture de stock" => "FF0000",
                    "Stock Faible" => "FFA500",
                    "En Stock" => "008000",
                    _ => "FFFFFF"
                };
            }
        }

        public ICommand? ViewInventoryCommand { get; set; }
      
        public PartInventoryItem(PartDto? partDto)
        {
            Part = partDto;
            PartNumber = partDto?.PartNumber;
            Name = partDto?.Name;
            Id = partDto?.Id ?? Guid.Empty;
            FullName = $"{PartNumber} - {Name}";
            AvailableQuantity = partDto?.Inventories?.Sum(i => i.AvailableQuantity) ?? 0;
            SetStatus();
            ViewInventoryCommand = new RelayCommand(() => IoContainer.Application.GoToPage( ApplicationPage.Inventory,new InventoryPageViewModel { Part=Part}));
        }

        private void SetStatus()
        {
            if (AvailableQuantity == 0)
            {
                Status = "En Rupture de stock";
            }
            else if (AvailableQuantity <= 5)
            {
                Status = "Stock Faible";
            }
            else
            {
                Status = "En Stock";
            }
        }
    }
}
