using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class InitialEquipmentViewModel : PropertyValidation
    {
        [Required(ErrorMessage = $"\uf06a  Nom de l'équipement Doit être Précisé")]
        public string EquipmentName { get => GetValue(() => EquipmentName); set { SetValue(() => EquipmentName, value); } }

        [Required(ErrorMessage = $"\uf06a  Description de l'équipement Doit être Précisé")]
        public string Description { get => GetValue(() => Description); set { SetValue(() => Description, value); } }

        public ICommand AddCommand { get; set; }

        public event EventHandler<EquipmentEventArgs> EquipmentAdded;


       
        private bool _DimmableOverlayVisible { get; set;}

        public bool DimmableOverlayVisible { get => _DimmableOverlayVisible; set { _DimmableOverlayVisible = value; OnPropertyChanged(nameof(DimmableOverlayVisible)); } }

        public bool IsBtnEnabled => !DimmableOverlayVisible;

        public InitialEquipmentViewModel()
        {
            AddCommand = new RelayParameterizedCommand( (p) => AddEquipment(p));
            
        }

        private async Task AddEquipment(object? p)
        {
           
            if (p is Window window)
            {
                if (IsValid)
                {
                    DimmableOverlayVisible = true;
                    // Add the equipment
                    //OnEquipmentAdded(new Equipment
                    //{
                    //    Id = Guid.NewGuid(),
                    //    EquipmentName = EquipmentName
                    //});
                    var response = await AppServices.ServiceManager.EquipmentService.CreateEquipmentAsync(new EquipmentForCreationDto
                    (
                        //Id : Guid.NewGuid(),
                        EquipmentParent: null,
                        EquipmentName: EquipmentName,
                        EquipmentType: null,
                        EquipmentDescription: Description,
                        EquipmentOrganization: null,
                        EquipmentDepartment: null,
                        EquipmentClass: null,
                        EquipmentSite: null,
                        EquipmentStatus: null,
                        EquipmentMake: null,
                        EquipmentSerialNumber: null,
                        EquipmentModel: null,
                        EquipmentCost: null,
                        EquipmentCommissionDate: null,
                        EquipmentAssignedTo: null,
                        EquipmentNameImage: null,
                        EquipmentImage: null

                    ));
                    if (response.Success)
                    {
                        await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Equipement Ajouté avec Succès"));
                    }
                    if (response is ApiOkResponse<EquipmentDto> okResponse)
                    {
                        var Result = okResponse.Result;
                    }
                    await Task.Delay(1000);
                    DimmableOverlayVisible = false;
                    ClearValidation();
                    window.Close();
                }else
                {
                   await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez Remplir les champs obligatoires"));
                }
            }
           
        }
        public void OnEquipmentAdded(Equipment equipment)
        {
            EquipmentAdded?.Invoke(this, new EquipmentEventArgs { AddEquipment = equipment });
        }
    }
}
