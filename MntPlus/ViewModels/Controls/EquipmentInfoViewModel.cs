using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class EquipmentInfoViewModel : BaseViewModel
    {
        public List<EquipmentType>? EquipmentTypes { get; set; }
        public EquipmentType? SelectedEquipmentType { get; set; }

        public ObservableCollection<Organization>? Organizations { get; set; }
        public Organization? SelectedOrganization { get; set; }

        public ObservableCollection<EquipmentDepartment>? Departments { get; set; }
        public EquipmentDepartment? SelectedDepartment { get; set; }

        public ObservableCollection<EquipmentClass>? EquipmentClasses { get; set; }
        public EquipmentClass? SelectedEquipmentClass { get; set; }

        public List<EquipmentStatus>? EquipmentStatuses { get; set; }
        public EquipmentStatus? SelectedEquipmentStatus { get; set; }

        public ObservableCollection<Assignee>? Assignees { get; set; }
        public Assignee? SelectedAssignee { get; set; }

        public string EquipmentName { get; set; }
        public string? EquipmentDescription { get; set; }
        public string? EquipmentSite { get; set; }
        public string? EquipmentMake { get; set; }
        public string? EquipmentSerialNumber { get; set; }
        public string? EquipmentModel { get; set; }
        public double? EquipmentCost { get; set; }
        public DateTime? EquipmentCommissionDate { get; set; }


        public string? AssigneeNameAdded { get; set; }
        public EquipmentDto Equipment { get; set; }


        public bool AssignPopup { get; set; }
        public bool DimmableOverlayVisible => AssignPopup;
        public ICommand AddAssigneeCommand { get; set; }
        public ICommand CloseAddAssigneeCommand { get; set; }
        public EquipmentInfoViewModel(/*EquipmentDto equipment*/)
        {
           // Equipment = equipment;
           // MapEquipmentDtoToEquipmentInfoViewModel();
            AddAssigneeCommand = new RelayCommand(() => AssignPopup = true);
            CloseAddAssigneeCommand = new RelayCommand(() => AssignPopup = false) ;
        }

      

        //map properties from EquipmentDto to EquipmentInfoViewModel
        private void MapEquipmentDtoToEquipmentInfoViewModel()
        {
            EquipmentName = Equipment.EquipmentName;
            EquipmentDescription = Equipment.EquipmentDescription;
            EquipmentSite = Equipment.EquipmentSite;
            EquipmentMake = Equipment.EquipmentMake;
            EquipmentSerialNumber = Equipment.EquipmentSerialNumber;
            EquipmentModel = Equipment.EquipmentModel;
            EquipmentCost = Equipment.EquipmentCost;
            EquipmentCommissionDate = Equipment.EquipmentCommissionDate;
            if(Equipment.EquipmentType is not null)
            {
                SelectedEquipmentType.Id = Equipment.EquipmentType.Id;
                SelectedEquipmentType.EquipmentTypeName = Equipment.EquipmentType.EquipmentTypeName;

            }
            if(Equipment.EquipmentOrganization is not null)
            {
                SelectedOrganization.Id = Equipment.EquipmentOrganization.Id;
                SelectedOrganization.OrganizationName = Equipment.EquipmentOrganization.OrganizationName;

            }
            if(Equipment.EquipmentDepartment is not null)
            {
                SelectedDepartment.Id = Equipment.EquipmentDepartment.Id;
                SelectedDepartment.DepartmentName = Equipment.EquipmentDepartment.DepartmentName;

            }
            if(Equipment.EquipmentClass is not null)
            {
                SelectedEquipmentClass.Id = Equipment.EquipmentClass.Id;
                SelectedEquipmentClass.EquipmentClassName = Equipment.EquipmentClass.ClassName;

            }
            if(Equipment.EquipmentStatus is not null)
            {
                SelectedEquipmentStatus.Id = Equipment.EquipmentStatus.Id;
                SelectedEquipmentStatus.EquipmentStatusName = Equipment.EquipmentStatus.StatusName;
            }
            if(Equipment.EquipmentAssignedTo is not null)
            {
                SelectedAssignee.Id = Equipment.EquipmentAssignedTo.Id;
                SelectedAssignee.Name = Equipment.EquipmentAssignedTo.AssignedToName;
            }
           
        }
           
    }
}
