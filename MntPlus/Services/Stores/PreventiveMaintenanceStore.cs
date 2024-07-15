using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class PreventiveMaintenanceStore
    {
        public event Action<PreventiveMaintenanceDto?>? PreventiveMaintenanceCreated;
        public event Action<PreventiveMaintenanceDto?>? PreventiveMaintenanceUpdated;
        public event Action<PreventiveMaintenanceDto?>? PreventiveMaintenanceDeleted;

        public void CreatePreventiveMaintenance(PreventiveMaintenanceDto? preventiveMaintenance)
        {
            PreventiveMaintenanceCreated?.Invoke(preventiveMaintenance);
        }

        public void UpdatePreventiveMaintenance(PreventiveMaintenanceDto? preventiveMaintenance)
        {
            PreventiveMaintenanceUpdated?.Invoke(preventiveMaintenance);
        }

        public void DeletePreventiveMaintenance(PreventiveMaintenanceDto? preventiveMaintenance)
        {
            PreventiveMaintenanceDeleted?.Invoke(preventiveMaintenance);
        }
    }
}
