using Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {
        public List<EquipmentType> EquipmentTypes { get; set; }
        public EquipmentType SelectedEquipmentType { get; set; }

        public ObservableCollection<Organization> Organizations { get; set; }
        public Organization SelectedOrganization { get; set; }

        public ObservableCollection<EquipmentDepartment> Departments { get; set; }
        public EquipmentDepartment SelectedDepartment { get; set; }

        public ObservableCollection<EquipmentClass> EquipmentClasses { get; set; }
        public EquipmentClass SelectedEquipmentClass { get; set; }

        public List<EquipmentStatus> EquipmentStatuses { get; set; }
        public EquipmentStatus SelectedEquipmentStatus { get; set; }
    }
}
