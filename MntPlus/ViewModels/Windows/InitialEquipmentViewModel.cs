using Entities;
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

        public InitialEquipmentViewModel()
        {
            AddCommand = new RelayParameterizedCommand( (p) => AddEquipment(p));
            
        }

        private void AddEquipment(object? p)
        {
            if(p is Window window)
            {
                if (IsValid)
                {
                    // Add the equipment
                    OnEquipmentAdded(new Equipment
                    {
                        Id = Guid.NewGuid(),
                        EquipmentName = EquipmentName
                    });
                    ClearValidation();
                    window.Close();
                }else
                {
                    IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez Remplir les champs obligatoires"));
                }
            }
           
        }
        public void OnEquipmentAdded(Equipment equipment)
        {
            EquipmentAdded?.Invoke(this, new EquipmentEventArgs { AddEquipment = equipment });
        }
    }
}
