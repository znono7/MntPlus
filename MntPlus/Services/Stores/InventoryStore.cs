using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class InventoryStore
    {
        public event Action<InventoryDto?>? InventoryCreated;

        public void CreateInventory(InventoryDto? inventory)
        {
            InventoryCreated?.Invoke(inventory);
        }

    }
}
